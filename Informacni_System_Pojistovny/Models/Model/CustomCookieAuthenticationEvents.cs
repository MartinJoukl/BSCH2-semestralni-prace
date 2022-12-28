namespace Informacni_System_Pojistovny.Models.Model
{
    using Informacni_System_Pojistovny.Models.Dao;
    using Informacni_System_Pojistovny.Models.Entity;
    using Informacni_System_Pojistovny.Models.Model.Uzivatele;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Oracle.ManagedDataAccess.Client;
    using System.Security.Claims;

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
            var parameters = new Dictionary<string, object>();

            var originalId = (from c in userPrincipal.Claims
                              where c.Type == "originalId"
                              select c.Value).FirstOrDefault();
            bool impersonification = originalId != null;
            //if lastChangedOring != null, impersonification is active
            if (originalId != null)
            {
                var lastChangedOring = (from c in userPrincipal.Claims
                                        where c.Type == "originalCasZmeny"
                                        select c.Value).FirstOrDefault();
                parameters.Add("id", originalId);
                DateTime.TryParse(lastChangedOring, out DateTime dateTimeClaimOrig);
                bool signedOut = await SignOutIfDateTimeDiffers(context, casZmeny, parameters, dateTimeClaimOrig, false);
                if (signedOut) { return; }
                parameters.Remove("id");
            }

            // Look for the LastChanged claim.
            var lastChanged = (from c in userPrincipal.Claims
                               where c.Type == "CasZmeny"
                               select c.Value).FirstOrDefault();
            DateTime.TryParse(lastChanged, out DateTime dateTimeClaim);
            var id = (from c in userPrincipal.Claims
                      where c.Type == "Id"
                      select c.Value).FirstOrDefault();
            parameters.Add("id", id);

            await SignOutIfDateTimeDiffers(context, casZmeny, parameters, dateTimeClaim, impersonification, id);
        }

        private async Task<bool> SignOutIfDateTimeDiffers(CookieValidatePrincipalContext context, DateTime casZmeny, Dictionary<string, object> parameters, DateTime dateTimeClaim, bool updateImpersonification, string id = null)
        {
            //read last time changed
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
                if (updateImpersonification && id != null)
                {
                    parameters = await ChangeImpersonificationInfo(context, id);
                }
                else
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    return true;
                }
            }

            return false;
        }

        private async Task<Dictionary<string, object>> ChangeImpersonificationInfo(CookieValidatePrincipalContext context, string id)
        {
            Dictionary<string, object> parameters = new();
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel impersonifikovany = uzivatelModel.GetUzivatel(int.Parse(id));
            var userPrincipal = context.Principal;

            parameters.Add("id", id);

            if (impersonifikovany == null)
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
                throw new Exception("Impersonifikace selhala!");
            }


            var claims = new List<Claim>
                 {
                 new Claim(ClaimTypes.Email, impersonifikovany.Email),
                 new Claim(ClaimTypes.Name, impersonifikovany.Jmeno),
                 new Claim(ClaimTypes.Surname, impersonifikovany.Prijmeni),
                 new Claim(ClaimTypes.Role, impersonifikovany.Role.ToString()),
                 new Claim("CasZmeny", impersonifikovany.casZmeny.ToString()),
                 new Claim("Id", impersonifikovany.Id.ToString())
                 };
            string originalMail = userPrincipal.Claims.Where((claim) => claim.Type == "originalMail").First().Value;
            string originalRole = userPrincipal.Claims.Where((claim) => claim.Type == "originalRole").First().Value;
            string originalCasZmeny = userPrincipal.Claims.Where((claim) => claim.Type == "originalCasZmeny").First().Value;
            string originalId = userPrincipal.Claims.Where((claim) => claim.Type == "originalId").First().Value;

            claims.Add(new Claim("originalMail", originalMail));
            claims.Add(new Claim("originalRole", originalRole));
            claims.Add(new Claim("originalCasZmeny", originalCasZmeny));
            claims.Add(new Claim("originalId", originalId));


            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();
            await context.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return parameters;
        }
    }
}
