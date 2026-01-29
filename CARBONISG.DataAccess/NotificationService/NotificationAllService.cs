using CARBONISG.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class NotificationAllService
  {
    public static List<NotificationModel> NotificationAllList(int companyid,NotificationType? type, NotificationPriority? priority,bool? status)
    {
      using (var context = new HcDbContext())
      {

        var complaints = context.ComplaintAndWishNotification
            .Join(
                context.Users,
                complaint => complaint.UserID, 
                user => user.ID, 
                (c, user) => new NotificationModel
                {
                  Id = c.ID,
                  UserID = c.UserID,
                  CompanyID=c.CompanyID,
                  FullName = c.IsNameVisible ? user.Name + " " + user.Surname : "Gizli Kullanıcı",
                  Email = c.IsNameVisible ? user.Email : "Gizli Kullanıcı",
                  Phone = c.IsNameVisible ? user.Phone : "Gizli Kullanıcı",
                  JobTitle = c.IsNameVisible ? user.JobTitle : "Gizli Kullanıcı",
                  Title = c.Suggestions,
                  Description = c.ComplaintOrWishDescription,
                  Date = c.ReportedAtTime,
                  Priority=c.Priority,
                  Type = NotificationType.ComplaintAndWish,
                  ComplaintOrWish = c.ComplaintOrWish,
                  IsActive=c.IsActive
                }
            );



        var incidents = context.IncidentNotification
              .Join(
                  context.Users,
                  incident => incident.UserID,
                  user => user.ID,
                  (i, user) => new NotificationModel
                  {
                    Id = i.ID,
                    UserID = i.UserID,
                    CompanyID = i.CompanyID,
                    FullName = i.IsNameVisible ? user.Name + " " + user.Surname : "Gizli Kullanıcı",
                    Email = i.IsNameVisible ? user.Email : "Gizli Kullanıcı",
                    Phone = i.IsNameVisible ? user.Phone : "Gizli Kullanıcı",
                    JobTitle = i.IsNameVisible ? user.JobTitle : "Gizli Kullanıcı",
                    Title = i.IncidentSection,
                    Description = i.IncidentDescription,
                    Date = i.ReportedAtTime,
                    Priority = i.Priority,
                    Type = NotificationType.Incident,
                    ComplaintOrWish = null,
                    IsActive = i.IsActive
                  }
              );

        var nearMisses = context.NearMissNotification
         .Join(
             context.Users,
             nearMiss => nearMiss.UserID,
             user => user.ID,
             (n, user) => new NotificationModel
             {
               Id = n.ID,
               UserID = n.UserID,
               CompanyID = n.CompanyID,
               FullName = n.IsNameVisible ? user.Name + " " + user.Surname : "Gizli Kullanıcı",
               Email = n.IsNameVisible ? user.Email : "Gizli Kullanıcı",
               Phone = n.IsNameVisible ? user.Phone : "Gizli Kullanıcı",
               JobTitle = n.IsNameVisible ? user.JobTitle : "Gizli Kullanıcı",
               Title = n.NearMissLocation,
               Description = n.NearMissDescription,
               Date = n.NearMissDate,
               Priority = n.Priority,
               Type = NotificationType.NearMiss,
               ComplaintOrWish = null,
               IsActive = n.IsActive
             }
         );

        var infringements = context.InfringementNotification
        .Join(
            context.Users,
            infringement => infringement.UserID,
            user => user.ID,
            (i, user) => new NotificationModel
            {
              Id = i.ID,
              UserID = i.UserID,
              CompanyID = i.CompanyID,
              FullName = i.IsNameVisible ? user.Name + " " + user.Surname : "Gizli Kullanıcı",
              Email = i.IsNameVisible ? user.Email : "Gizli Kullanıcı",
              Phone = i.IsNameVisible ? user.Phone : "Gizli Kullanıcı",
              JobTitle = i.IsNameVisible ? user.JobTitle : "Gizli Kullanıcı",
              Title = i.InfringementLocation,
              Description = i.InfringementDescription,
              Date = i.ReportedAtTime,
              Priority = i.Priority,
              Type = NotificationType.Infringement,
              ComplaintOrWish = null,
              IsActive = i.IsActive
            }
        );

        IEnumerable<NotificationModel> allNotifications = Enumerable.Empty<NotificationModel>();

        if (type == NotificationType.All || !type.HasValue)
        {
          allNotifications = complaints
              .Concat(incidents)
              .Concat(nearMisses)
              .Concat(infringements);
        }
        else if (type == NotificationType.ComplaintAndWish)
        {
          allNotifications = complaints;
        }
        else if (type == NotificationType.Incident)
        {
          allNotifications = incidents;
        }
        else if (type == NotificationType.NearMiss)
        {
          allNotifications = nearMisses;
        }
        else if (type == NotificationType.Infringement)
        {
          allNotifications = infringements;
        }


        var filteredNotifications = allNotifications.Where(n => n.CompanyID == companyid);

        if (type.HasValue && type != NotificationType.All)
        {
          filteredNotifications = filteredNotifications.Where(n => n.Type == type.Value);
        }


        if (priority.HasValue)
        {
          filteredNotifications = filteredNotifications.Where(n => n.Priority == priority.Value);
        }

        if (status.HasValue)
        {
          filteredNotifications = filteredNotifications.Where(n => n.IsActive == status.Value);
        }
        return filteredNotifications
            .OrderByDescending(n => n.Date)
            .ToList();
      }
    }

    public static bool UpdatePriority(int id, NotificationType type, NotificationPriority priority)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (id > 0)
          {
            if (type == NotificationType.ComplaintAndWish)
            {
              var existingModel = context.ComplaintAndWishNotification.FirstOrDefault(p => p.ID == id);
              if (existingModel != null)
              {
                existingModel.Priority = priority;
              }
            }
            else if (type == NotificationType.NearMiss)
            {
              var existingModel = context.NearMissNotification.FirstOrDefault(p => p.ID == id);
              if (existingModel != null)
              {
                existingModel.Priority = priority;
              }
            }
            else if (type == NotificationType.Incident)
            {
              var existingModel = context.IncidentNotification.FirstOrDefault(p => p.ID == id);
              if (existingModel != null)
              {
                existingModel.Priority = priority;
              }
            }
            else if (type == NotificationType.Infringement)
            {
              var existingModel = context.InfringementNotification.FirstOrDefault(p => p.ID == id);
              if (existingModel != null)
              {
                existingModel.Priority = priority;
              }
            }

            context.SaveChanges();
            return true;
          }
          return false;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

    public static int GetTotalNotification(int? companyId, bool isMaster)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          int totalNotifications = 0;

          if (isMaster)
          {
            totalNotifications = context.NearMissNotification.Count()
                                + context.InfringementNotification.Count()
                                + context.IncidentNotification.Count()
                                + context.ComplaintAndWishNotification.Count();
          }
          else if (companyId.HasValue)
          {
            totalNotifications = context.NearMissNotification.Count(n => n.CompanyID == companyId)
                                + context.InfringementNotification.Count(n => n.CompanyID == companyId)
                                + context.IncidentNotification.Count(n => n.CompanyID == companyId)
                                + context.ComplaintAndWishNotification.Count(n => n.CompanyID == companyId);
          }

          return totalNotifications;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Bildirim sayısı alınırken hata oluştu.", ex);
      }
    }


    public static int GetAnonymousNotifications(int? companyId, bool isMaster)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          int totalNotifications = 0;

          if (isMaster)
          {
            totalNotifications = context.NearMissNotification.Where(p=>p.IsNameVisible==false).Count()
                                + context.InfringementNotification.Where(p => p.IsNameVisible == false).Count()
                                + context.IncidentNotification.Where(p => p.IsNameVisible == false).Count()
                                + context.ComplaintAndWishNotification.Where(p => p.IsNameVisible == false).Count();
          }
          else if (companyId.HasValue)
          {
            totalNotifications = context.NearMissNotification.Where(p => p.IsNameVisible == false).Count(n => n.CompanyID == companyId)
                                + context.InfringementNotification.Where(p => p.IsNameVisible == false).Count(n => n.CompanyID == companyId)
                                + context.IncidentNotification.Where(p => p.IsNameVisible == false).Count(n => n.CompanyID == companyId)
                                + context.ComplaintAndWishNotification.Where(p => p.IsNameVisible == false).Count(n => n.CompanyID == companyId);
          }

          return totalNotifications;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Bildirim sayısı alınırken hata oluştu.", ex);
      }
    }

        public static async Task<Dictionary<string, int>> GetNotificationPriorityCounts(int companyId)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    var infringementCount = await context.InfringementNotification
                        .Where(i => i.CompanyID == companyId)
                        .GroupBy(i => i.Priority)
                        .Select(g => new { Priority = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var nearMissCount = await context.NearMissNotification
                        .Where(n => n.CompanyID == companyId)
                        .GroupBy(n => n.Priority)
                        .Select(g => new { Priority = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var incidentCount = await context.IncidentNotification
                        .Where(i => i.CompanyID == companyId)
                        .GroupBy(i => i.Priority)
                        .Select(g => new { Priority = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var complaintAndWishCount = await context.ComplaintAndWishNotification
                        .Where(c => c.CompanyID == companyId)
                        .GroupBy(c => c.Priority)
                        .Select(g => new { Priority = g.Key, Count = g.Count() })
                        .ToListAsync();

                    var notificationCounts = new Dictionary<string, int>
            {
                { "Low", infringementCount.Concat(nearMissCount).Concat(incidentCount).Concat(complaintAndWishCount).Where(g => g.Priority == NotificationPriority.Low).Sum(g => g.Count) },
                { "Medium", infringementCount.Concat(nearMissCount).Concat(incidentCount).Concat(complaintAndWishCount).Where(g => g.Priority == NotificationPriority.Medium).Sum(g => g.Count) },
                { "High", infringementCount.Concat(nearMissCount).Concat(incidentCount).Concat(complaintAndWishCount).Where(g => g.Priority == NotificationPriority.High).Sum(g => g.Count) }
            };

                    return notificationCounts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
