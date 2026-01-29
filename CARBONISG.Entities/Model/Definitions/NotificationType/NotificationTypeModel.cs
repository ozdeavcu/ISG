using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CARBONISG.Entities
{
  public class NotificationTypeModel
  {
    public int ID { get; set; }
    [Required]
    public NotificationType Type { get; set; }
    //public string TypeName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } //oluşturma tarihi
    public DateTime UpdateAt { get; set; }
    public string PictureUrl { get; set; }
    [NotMapped]
    public IFormFile PictureUrlUpload { get; set; }
    public bool IsActive { get; set; }
    }
  public enum NotificationType
  {
    [Display(Name = "Tümü")]
    All = 0,

    [Display(Name = "Dilek ve Şikayet")]
    ComplaintAndWish = 1,

    [Display(Name = "Olay Bildirim")]
    Incident = 2,

    [Display(Name = "Uygunsuzluk ve Risk Bildirim")]
    Infringement = 3,

    [Display(Name = "Ramak Kala")]
    NearMiss = 4
  }

  public enum NotificationPriority
  {
    [Display(Name = "Düşük")]
    Low = 1,

    [Display(Name = "Orta")]
    Medium =2,

    [Display(Name = "Yüksek")]
    High =3,
  }
}
