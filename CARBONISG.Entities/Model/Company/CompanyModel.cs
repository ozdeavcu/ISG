
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;


namespace CARBONISG.Entities
{
  public class CompanyModel
  {
    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    [StringLength(15)]
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public string? Tax { get; set; }
    public int? Sector { get; set; }
    public string? CompanyLogo { get; set; }
    public string? LogoData { get; set; }
    [NotMapped]
    public IFormFile LogoUpload { get; set; }
    public string? Website { get; set; }
    public int? PackageId { get; set; }
    public string? BillingAddress { get; set; }//fatura adresi
    public int? NumberOfEmployees { get; set; }//çalışan sayısı
    public int? CompanyType { get; set; }//şirket türü (kobi,kurumsal)
    public string Province { get; set; } //il
    public string District { get; set; } //ilçe
    public string Street { get; set; } //mahalle 
    public string? CompanyAuthorizedName { get; set; } //yetkili adı
    public string? CompanyAuthorizedSurname { get; set; }
  }
}
