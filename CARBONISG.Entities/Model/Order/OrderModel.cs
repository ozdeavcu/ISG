

namespace CARBONISG.Entities
{
  public class OrderModel
  {
    public int ID { get; set; }
    public string OrderNumber { get; set; }
    public int UserID { get; set; }
    public int CompanyID { get; set; }
    public int PackageID { get; set; }
    public int InvoiceID { get; set; }
    public DateTime OrderDate { get; set; }//sipariş oluşturulma tarihi
    public DateTime EndDate { get; set; } //paket bitiş tarihi
    public decimal TotalAmount { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }
    public string UserIP { get; set; }
    public bool IsActive { get; set; }

  }
  public enum PaymentStatusEnum
  {
    NotPaid = 0,
    Paid = 1,
    Pending = 2,  // Ödeme bekliyor
    Failed = 3,   // Ödeme başarısız
    Cancelled = 4 // Sipariş iptal edilmiş
  }
}
