
using System.ComponentModel.DataAnnotations.Schema;


namespace CARBONISG.Entities
{
  public class OrderCartModel
  {
    public int ID { get; set; }
    public int UserID { get; set; }  
    public int PackageID { get; set; }
    [NotMapped]
    public string PackageName { get; set; }
    public DateTime CreateDate { get; set; }
  }
}
