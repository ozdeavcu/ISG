using System.ComponentModel.DataAnnotations.Schema;


namespace CARBONISG.Entities
{
  public class InvoiceAddressModel
  {
    public int ID { get; set; }   
    public int UserID { get; set; }
    public string? SellerTaxNumber { get; set; }  // Satıcı vergi numarası
    public string? SellerTaxOffice { get; set; }  // Satıcı vergi dairesi
    public string? PostalCode { get; set; }
    public string BillingAddress { get; set; }  // Fatura adresi.
    public string Country { get; set; } //ülke
    public string City { get; set; }  // şehir
    public string District { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    [NotMapped]
    public string Phone { get; set; }
    [NotMapped]
    public string Name { get; set; }
    [NotMapped]
    public string Surname { get; set; }
    [NotMapped]
    public string Email { get; set; }
    [NotMapped]
    public string? IdentityNumber { get; set; }
  }
}
