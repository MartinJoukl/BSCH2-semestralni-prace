using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace Informacni_System_Pojistovny.Models.Model.Uzivatele
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

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from uzivatele_view");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string mail = dr["mail"]?.ToString();
                    string jmeno = dr["jmeno"]?.ToString();
                    string prijmeni = dr["prijmeni"]?.ToString();
                    string role = dr["uzivatel_role"]?.ToString();
                    string idString = dr["id"]?.ToString();
                    DateTime.TryParse(dr["cas_zmeny"]?.ToString(), out DateTime casZmeny);
                    int.TryParse(idString, out var id);
                    list.Add(new Uzivatel
                    {
                        Email = mail,
                        Jmeno = jmeno,
                        Prijmeni = prijmeni,
                        Role = UzivateleRoleRetriever.GetByName(role),
                        Id = id,
                        casZmeny = casZmeny
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public List<Uzivatel> ListUzivatel(PageInfo pageInfo)
        {
            List<Uzivatel> list = new List<Uzivatel>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from uzivatele_view order by id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY", parameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string mail = dr["mail"]?.ToString();
                    string jmeno = dr["jmeno"]?.ToString();
                    string prijmeni = dr["prijmeni"]?.ToString();
                    string role = dr["uzivatel_role"]?.ToString();
                    string idString = dr["id"]?.ToString();
                    DateTime.TryParse(dr["cas_zmeny"]?.ToString(), out DateTime casZmeny);
                    int.TryParse(idString, out var id);
                    list.Add(new Uzivatel
                    {
                        Email = mail,
                        Jmeno = jmeno,
                        Prijmeni = prijmeni,
                        Role = UzivateleRoleRetriever.GetByName(role),
                        Id = id,
                        casZmeny = casZmeny
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public long GetCount()
        {
            long count = 0;
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select count(*) as count from uzivatele_view");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    count = long.Parse(dr["count"].ToString());
                }
            }
            dr.Close();
            db.Dispose();
            return count;
        }

        public Uzivatel? EditUzivatel(EditUserModel model, int id)
        {
            HashSalt hashSalt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("p_id", id);

            if (!string.IsNullOrEmpty(model.Mail))
            {
                parameters.Add("p_mail", model.Mail);
            }
            if (!string.IsNullOrEmpty(model.Jmeno))
            {
                parameters.Add("p_jmeno", model.Jmeno);
            }
            if (!string.IsNullOrEmpty(model.Prijmeni))
            {
                parameters.Add("p_prijmeni", model.Prijmeni);
            }
            if (!string.IsNullOrEmpty(model.Heslo))
            {
                hashSalt = GenerateSaltedHash(100, model.Heslo);
                parameters.Add("p_hesloHash", hashSalt.Hash);
                parameters.Add("p_hesloSalt", hashSalt.Salt);
            }

            db.ExecuteNonQuery("update_uzivatel", parameters, false, true);
            db.Dispose();
            return null;
        }

        public Uzivatel? Login(string mail, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":mail", mail);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele_view where LOWER(mail) = LOWER(:mail)", parameters);
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
                    Role = UzivateleRoleRetriever.GetByName(recievedResult["uzivatel_role"]?.ToString()),
                    casZmeny = casZmeny
                };

                if (VerifyPassword(password, user.HesloHash, user.Salt))
                {
                    db.Dispose();
                    return user;
                }
            }
            db.Dispose();
            return null;
        }

        public Uzivatel? ForceLogin(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":id", id);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele_view where id = :id", parameters);
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
                    Role = UzivateleRoleRetriever.GetByName(recievedResult["uzivatel_role"]?.ToString()),
                    casZmeny = casZmeny
                };
                db.Dispose();
                return user;
            }
            db.Dispose();
            return null;
        }

        public Uzivatel? GetUzivatel(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":id", id);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele_view where id = :id", parameters);
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
                    Role = UzivateleRoleRetriever.GetByName(recievedResult["uzivatel_role"]?.ToString()),
                    casZmeny = casZmeny
                };
                db.Dispose();
                return user;
            }
            db.Dispose();
            return null;
        }

        public void DeleteUzivatel(int id)
        {
            db.ExecuteNonQuery("DELETE_UZIVATEL", new Dictionary<string, object>() { { "p_id", id } }, false, true);
        }

        public void Create(UzivatelRegisterFormModel uzivatelRegisterModel)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":mail", uzivatelRegisterModel.Mail);
            var recievedResult = db.ExecuteRetrievingCommand("Select * from uzivatele_view where mail = :mail", parameters);
            if (recievedResult.HasRows)
            {
                throw new Exception("Uživatel se stejným mailem už je registrován!");
            }
            HashSalt hashSalt = GenerateSaltedHash(100, uzivatelRegisterModel.Heslo);
            //int id = 0;
            parameters = new Dictionary<string, object>
            {
                { "p_mail", uzivatelRegisterModel.Mail },
                { "p_jmeno", uzivatelRegisterModel.Jmeno },
                { "p_prijmeni", uzivatelRegisterModel.Prijmeni },
                { "p_heslohash", hashSalt.Hash },
                { "p_salt", hashSalt.Salt }
            };
            //parameters.Add("p_out_id", id);

            db.ExecuteNonQuery("CREATE_UZIVATEL", parameters, false, true);
            db.Dispose();
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
