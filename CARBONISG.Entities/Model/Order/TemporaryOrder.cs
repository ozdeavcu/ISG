using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
  public class TemporaryOrderModel
  {
    [Key]
    public int ID { get; set; }
    [Required]
    public string GuidNumber { get; set; }
    [Required]
    public string OrderNumber { get; set; }
    public int UserID { get; set; }
    public int CompanyID { get; set; }
    public int PackageID { get; set; }
    public int InvoiceID { get; set; }
    public string UserIP { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }//sipariş oluşturulma tarihi
    public DateTime EndDate { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }
    public bool IsActive { get; set; } = false;

  }
}
