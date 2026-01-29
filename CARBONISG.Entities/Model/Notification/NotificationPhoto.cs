using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
  public class NotificationPhoto
  {
    public int ID { get; set; }
    public int UserID { get; set; }
    public int NotificationID { get; set; }
    public int CompanyID { get; set; }
    public string PhotoUrl { get; set; }
    public string PhotoData { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public NotificationType Type { get; set; }
  }
}
