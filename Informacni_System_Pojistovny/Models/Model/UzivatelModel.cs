using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelModel
    {
        public List<Uzivatel> ListUzivatel()
        {
            List<Uzivatel> list = new List<Uzivatel>();

            Db db = new Db();
            OracleDataReader dr = db.executeRetrievingCommand("select * from uzivatele");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string mail = dr["mail"]?.ToString();
                    string jmeno = dr["jmeno"]?.ToString();
                    string prijmeni = dr["prijmeni"]?.ToString();
                    string urovenOpravneniString = dr["uroven_opravneni"]?.ToString();
                    string idString = dr["id"]?.ToString();
                    int.TryParse(idString, out var id);
                    int.TryParse(urovenOpravneniString, out var urovenOpravneni);
                    list.Add(new Uzivatel { Mail = mail, Jmeno = jmeno, Prijmeni = prijmeni, UrovenOpravneni = urovenOpravneni, ID = id });
                }
            }
            db.Dispose();
            return list;
        }

        public Uzivatel? Login(string mail, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":mail", mail);
            Db db = new Db();
            var recievedResult = db.executeRetrievingCommand($"Select * from uzivatele where mail = :mail", parameters);
            if (recievedResult.HasRows)
            {
                Uzivatel user = new Uzivatel
                {
                    Jmeno = recievedResult["jmeno"]?.ToString(),
                    Mail = recievedResult["mail"]?.ToString(),
                    Prijmeni = recievedResult["prijmeni"]?.ToString(),
                    HesloHash = recievedResult["heslohash"]?.ToString(),
                    Salt = recievedResult["salt"]?.ToString(),
                    ID = int.Parse(recievedResult["ID"]?.ToString()),
                    UrovenOpravneni = int.Parse(recievedResult["uroven_opravneni"]?.ToString())
                };

                if (VerifyPassword(password, user.HesloHash, user.Salt))
                {
                    return user;
                }
            }
            return null;
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
