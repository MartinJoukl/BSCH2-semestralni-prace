namespace Informacni_System_Pojistovny.Models.Model
{
    using Informacni_System_Pojistovny.Models.Dao;
    using Informacni_System_Pojistovny.Models.Entity;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Oracle.ManagedDataAccess.Client;

    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly Db _db;

        public CustomCookieAuthenticationEvents(Db db)
        {
            _db = db;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;
            DateTime casZmeny = DateTime.MinValue; //temp

            // Look for the LastChanged claim.
            var lastChanged = (from c in userPrincipal.Claims
                               where c.Type == "CasZmeny"
                               select c.Value).FirstOrDefault();
            DateTime.TryParse(lastChanged, out DateTime dateTimeClaim);
            var id = (from c in userPrincipal.Claims
                      where c.Type == "Id"
                      select c.Value).FirstOrDefault();

            var parameters = new Dictionary<string, object>();
            parameters.Add(":id", id);

            OracleDataReader reader = _db.ExecuteRetrievingCommand("Select Cas_Zmeny from uzivatele where id = :id", parameters);
            if (reader.Read())
            {
                DateTime.TryParse(reader["cas_zmeny"]?.ToString(), out casZmeny);
            }
            else
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }


            if (!casZmeny.Equals(dateTimeClaim))
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
