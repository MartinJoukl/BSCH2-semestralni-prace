using System.ComponentModel;

namespace Informacni_System_Pojistovny.Models.Model.Uzivatele
{
    public class UzivateleRoleRetriever
    {
        public static UzivateleRole GetByName(string name)
        {
            UzivateleRole uzivateleRole;

            switch (name.ToLower())
            {
                case "admin":
                    uzivateleRole = UzivateleRole.Admin;
                    break;
                case "user":
                    uzivateleRole = UzivateleRole.User;
                    break;
                case "priviledgeduser":
                    uzivateleRole = UzivateleRole.PriviledgedUser;
                    break;
                case null:
                    uzivateleRole = UzivateleRole.No_Role; break;
                default: uzivateleRole = UzivateleRole.Unknown; break;
            }
            return uzivateleRole;
        }
    }
}
public enum UzivateleRole
{
    [Description("admin")]
    Admin,
    [Description("user")]
    User,
    [Description("priviledgedUser")]
    PriviledgedUser,
    [Description("Bez role")]
    No_Role,
    [Description("Neznámá role")]
    Unknown
}
