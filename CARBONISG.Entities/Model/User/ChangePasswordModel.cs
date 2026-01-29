using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
  public class ChangePasswordModel
  {
    public int Id { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string ExpirationTime { get; set; }
  }
}
