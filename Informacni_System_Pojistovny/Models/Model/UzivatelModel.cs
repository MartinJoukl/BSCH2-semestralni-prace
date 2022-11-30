using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelModel
    {
        private readonly Db db;
        public UzivatelModel(Db db)
        {
            this.db = db;
        }
        public List<Uzivatel> ListUzivatel()
        {
            List<Uzivatel> list = new List<Uzivatel>();

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from uzivatele");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string mail = dr["mail"]?.ToString();
                    string jmeno = dr["jmeno"]?.ToString();
                    string prijmeni = dr["prijmeni"]?.ToString();
                    string urovenOpravneniString = dr["uroven_opravneni"]?.ToString();
                    string idString = dr["id"]?.ToString();
                    DateTime.TryParse(dr["cas_zmeny"]?.ToString(), out DateTime casZmeny);
                    int.TryParse(idString, out var id);
                    int.TryParse(urovenOpravneniString, out var urovenOpravneni);
                    list.Add(new Uzivatel
                    {
                        Email = mail,
                        Jmeno = jmeno,
                        Prijmeni = prijmeni,
                        UrovenOpravneni = urovenOpravneni,
                        Id = id,
                        casZmeny = casZmeny
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public Uzivatel? Login(string mail, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":mail", mail);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele where LOWER(mail) = LOWER(:mail)", parameters);
            if (recievedResult.Read())
            {
                DateTime.TryParse(recievedResult["cas_zmeny"]?.ToString(), out DateTime casZmeny);
                Uzivatel user = new Uzivatel
                {
                    Jmeno = recievedResult["jmeno"]?.ToString(),
                    Email = recievedResult["mail"]?.ToString(),
                    Prijmeni = recievedResult["prijmeni"]?.ToString(),
                    HesloHash = recievedResult["heslohash"]?.ToString(),
                    Salt = recievedResult["salt"]?.ToString(),
                    Id = int.Parse(recievedResult["ID"]?.ToString()),
                    UrovenOpravneni = int.Parse(recievedResult["uroven_opravneni"]?.ToString()),
                    casZmeny = casZmeny
                };

                if (VerifyPassword(password, user.HesloHash, user.Salt))
                {
                    return user;
                }
            }
            return null;
        }

        public Uzivatel? Impersonifikuj(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":id", id);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele where id = :id", parameters);
            if (recievedResult.Read())
            {
                Uzivatel user = new Uzivatel
                {
                    Jmeno = recievedResult["jmeno"]?.ToString(),
                    Email = recievedResult["mail"]?.ToString(),
                    Prijmeni = recievedResult["prijmeni"]?.ToString(),
                    HesloHash = recievedResult["heslohash"]?.ToString(),
                    Salt = recievedResult["salt"]?.ToString(),
                    Id = int.Parse(recievedResult["ID"]?.ToString()),
                    UrovenOpravneni = int.Parse(recievedResult["uroven_opravneni"]?.ToString())
                };
                return user;
            }
            return null;
        }

        public int Register(UzivatelRegisterFormModel uzivatelRegisterModel)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":mail", uzivatelRegisterModel.Mail);
            Db db = new Db();
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele where mail = :mail", parameters);
            if (recievedResult.HasRows)
            {
                throw new Exception("Uživatel se stejným mailem už je registrován!");
            }
            HashSalt hashSalt = GenerateSaltedHash(100, uzivatelRegisterModel.Heslo);

            parameters = new Dictionary<string, object>();
            parameters.Add(":mail", uzivatelRegisterModel.Mail);
            parameters.Add(":jmeno", uzivatelRegisterModel.Jmeno);
            parameters.Add(":prijmeni", uzivatelRegisterModel.Prijmeni);
            parameters.Add(":heslohash", hashSalt.Hash);
            parameters.Add(":salt", hashSalt.Salt);

            return db.ExecuteNonQuery("INSERT into Uzivatele (mail,jmeno,prijmeni,heslohash,salt, uroven_opravneni) Values ( :mail, :jmeno, :prijmeni, :heslohash, :salt ,0) returning id into :id", parameters);
        }
        //SO :)
        public HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }
        //SO :D
        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
    }
}
