
namespace CARBONISG.Entities
{
  public class LoginViewModel
  {
    public string LoginIdentifier { get; set; }
    public string Password { get; set; }
    public AdminEnum IsAdmin { get; set; }
    public string Email { get; set; }
  }
}
