using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.Entities
{
  public class DashboardModel
  {
    public int TotalNotificationCompany {  get; set; }
    public int TotalNotificationMaster { get; set; }

    public int TotalAnonymousNotificationsMaster {  get; set; }
    public int TotalAnonymousNotificationsCompany { get; set; }

    public int TotalUserMaster { get; set; }
    public int TotalUserCompany { get; set; }

    public int NumberOfRemainingEmployees { get; set; }

    public int LowNotificationCount { get; set; }

    public int MediumNotificationCount { get; set; }

    public int HighNotificationCount { get; set; }




    }
}
