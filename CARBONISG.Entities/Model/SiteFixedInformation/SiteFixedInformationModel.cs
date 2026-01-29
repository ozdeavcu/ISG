using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CARBONISG.Entities
{
  public class SiteFixedInformationModel
  {
    public int ID { get; set; }
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    [StringLength(15)]
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? LinkedlnIcon { get; set; }
    public string? Linkedln { get; set; }
    public string? TwitterIcon { get; set; }
    public string? Twitter { get; set; }
    public string? InstagramIcon { get; set; }
    public string? Instagram { get; set; }
    public string? FacebookIcon { get; set; }
    public string? Facebook { get; set; }
    public string? YoutubeIcon { get; set; }
    public string? Youtube { get; set; }
    public string? WhatsappIcon { get; set; }
    [StringLength(15)]
    public string? WhatsappPhone { get; set; }
    public DateTime? UpdateDate { get; set; }
    [NotMapped]
    public IFormFile LogoUpload { get; set; }
    [NotMapped]
    public IFormFile LinkedlnIconUpload { get; set; }
    [NotMapped]
    public IFormFile TwitterIconUpload { get; set; }
    [NotMapped]
    public IFormFile InstagramIconUpload { get; set; }
    [NotMapped]
    public IFormFile FacebookIconUpload { get; set; }
    [NotMapped]
    public IFormFile YoutubeIconUpload { get; set; }
    [NotMapped]
    public IFormFile WhatsappIconUpload { get; set; }
  }
}
