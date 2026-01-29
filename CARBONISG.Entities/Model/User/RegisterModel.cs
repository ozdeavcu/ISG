
using System.Security.Cryptography;
using System.Text;


namespace CARBONISG.Entities
{
  public class RegisterModel
  {
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
    public string Phone { get; set; }
    public string CompanyAuthorizedName { get; set; }
    public string CompanyAuthorizedSurname { get; set; }
    public string CompanyName { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string Address { get; set; }

    public void HashPassword()
    {
      Password = HashPassword(Password);
    }

    private string HashPassword(string password)
    {
      using (SHA256 sha256Hash = SHA256.Create())
      {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in bytes)
        {
          builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
      }
    }
  }
}
