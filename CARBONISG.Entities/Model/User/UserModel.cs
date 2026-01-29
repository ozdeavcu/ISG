
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Security.Cryptography;
using System.Text;


namespace CARBONISG.Entities
{
  public class UserModel
  {
    [Key]
    public int ID { get; set; }
    public string? Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    [StringLength(15)]
    public string Phone { get; set; }
    public string? RoleId { get; set; }
    public int? CompanyId { get; set; }
    [NotMapped]
    public string? CompanyName { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsActive { get; set; }
    [DataType(DataType.Password)]
    public string? PasswordHash { get; set; }
    [DataType(DataType.Password)]
    [NotMapped]
    public string? ConfirmPassword { get; set; }
    public AdminEnum IsAdmin { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? JobTitle { get; set; }  
    public DateTime DateHired { get; set; } = DateTime.Now; //işe başlama tarihi 
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactName { get; set; }
    public DateTime? DateLeft { get; set; } //işten çıkış
    public int? Department { get; set; }
    [NotMapped]
    public string? DepartmentName { get; set; }
    public string? IdentityNumber { get; set; } //tc kimlik

    public void HashPassword()
    {
      PasswordHash = HashPassword(PasswordHash);
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
  public enum AdminEnum
  {
    [Display(Name = "Master Admin")]
    Master = 1,

    [Display(Name = "Yönetici")]
    Yonetici = 2,

    [Display(Name = "Çalışan")]
    Calisan = 3
  }
}
