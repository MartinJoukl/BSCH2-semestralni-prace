using System.ComponentModel;

namespace Informacni_System_Pojistovny.Models.Model
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
                default: throw new Exception("Role doesn't exist");
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
    PriviledgedUser
}
