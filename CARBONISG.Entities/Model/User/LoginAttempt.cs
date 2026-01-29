
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CARBONISG.Entities
{
  [Table("LoginAttempts")]
  public class LoginAttempt
  {
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public DateTime AttemptTime { get; set; }

    [MaxLength(45)]
    public string IPAddress { get; set; }
    public bool IsSuccess { get; set; }
  }
}
