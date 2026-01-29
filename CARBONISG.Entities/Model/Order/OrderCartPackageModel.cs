

namespace CARBONISG.Entities
{
  public class OrderCartPackageModel
  {
    public int OrderCartId {  get; set; }
    public int UserID { get; set; }
    public string OrderNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? CustomerFullName { get; set; }
    public string? CustomerPhone { get; set; }
    public int PackageID { get; set; }
    public string? PackageName { get; set; }
    public string? PackageDescription { get; set; }
    public int MaxEmployees { get; set; }
    public bool IsActive { get; set; }
    public decimal PackagePrice { get; set; }
    public decimal PackageVAT{ get; set; }
    public DateTime CreateDate { get; set; }
    public decimal TotalPrice { get; set; }
    public InvoiceAddressModel Invoice { get; set; }

  }
}
