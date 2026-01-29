
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CARBONISG.Entities
{
  public class InfringementNotificationModel
  {
    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public int CompanyID { get; set; }
    [NotMapped]
    public string? FullName { get; set; }
    [NotMapped]
    public string? Email { get; set; }
    [NotMapped]
    public string? Phone { get; set; }
    [NotMapped]
    public string? JobTitle { get; set; }
    //public int NotificationTypeID { get; set; }
    public DateTime ReportedAt { get; set; } //bildirim tarihi
    public DateTime ReportedAtTime { get; set; } //bildirim saati
    public bool IsNameVisible { get; set; }
    [NotMapped]
    public List<NotificationPhoto> Photos { get; set; } = new List<NotificationPhoto>();
    [MaxLength(500)]
    public string? AudioUrl { get; set; }
    public byte[]? AudioData { get; set; }
    public bool IsActive { get; set; }
    public DateTime? IsPassiveDate { get; set; }
    public string InfringementLocation { get; set; }
    public string InfringementSection { get; set; }
    public string InfringementDescription { get; set; }
    public string InfringementSolution { get; set; } //Uygunsuzluğun çözüm önerisi
    public DateTime ResolvedAt { get; set; } //Çözüme ulaştığı tarih
    public NotificationPriority? Priority { get; set; }
  }
}
