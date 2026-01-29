using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
  public class NotificationModel
  {
    public int Id { get; set; } 
    public int UserID { get; set; }
    public int CompanyID { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? JobTitle { get; set; }
    public DateTime Date { get; set; }
    public NotificationType Type { get; set; }
    public ComplaintOrWishEnum? ComplaintOrWish { get; set; }
    public NotificationPriority? Priority { get; set; }




    public DateTime ReportedAt { get; set; } //bildirim tarihi
    public DateTime ReportedAtTime { get; set; } //bildirim saati
    public bool IsNameVisible { get; set; }
    public string? PhotoUrl { get; set; }
    [MaxLength(500)]
    public string? AudioUrl { get; set; }
    public byte[]? AudioData { get; set; }
    public bool IsActive { get; set; }

    //dilek ve şikayet
    public string? ComplaintOrWishTitle { get; set; }
    public string? ComplaintOrWishDescription { get; set; }
    public string? Suggestions { get; set; }

    //olay bildirim
    public string? IncidentLocation { get; set; }
    public DateTime? IncidentDate { get; set; }
    public DateTime? IncidentTime { get; set; }
    public string? IncidentDescription { get; set; }

    //uygunsuzluk
    public string? InfringementLocation { get; set; }
    public string? InfringementDescription { get; set; }
    public string? InfringementSolution { get; set; } //Uygunsuzluğun çözüm önerisi
    public DateTime? ResolvedAt { get; set; }

    //ramak kala
    public string? NearMissLocation { get; set; } //Ramak kala bölümü ve yeri
    public DateTime? NearMissDate { get; set; }
    public DateTime? NearMissTime { get; set; }
    public string? NearMissDescription { get; set; }
  }
}
