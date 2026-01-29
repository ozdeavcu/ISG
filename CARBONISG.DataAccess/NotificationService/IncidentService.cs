using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class IncidentService
  {
    public List<IncidentNotificationModel> IncidentNotificationList()
    {
      using (var context = new HcDbContext())
      {
        return context.IncidentNotification.ToList();
      }
    }

    public bool Save(IncidentNotificationModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingModel = context.IncidentNotification.FirstOrDefault(p => p.ID == model.ID);

            if (existingModel != null)
            {
              existingModel.UserID = model.UserID;
              existingModel.ReportedAt = model.ReportedAt;
              existingModel.ReportedAtTime = model.ReportedAtTime;
              existingModel.IsNameVisible = model.IsNameVisible;
              existingModel.IsActive =  model.IsActive;
              existingModel.IncidentLocation = model.IncidentLocation;
              existingModel.IncidentDate = model.IncidentDate;
              existingModel.IncidentTime = model.IncidentTime;
              existingModel.IncidentDescription = model.IncidentDescription;
              existingModel.IncidentSection = model.IncidentSection;
            }
            else
            {
              model.IsActive = true;
              context.Add(model);
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

    public IncidentNotificationModel GetIncidentById(int id)
    {
      using (var context = new HcDbContext())
      {
        IncidentNotificationModel model = context.IncidentNotification.Find(id);
        return model;
      }
    }

    public List<IncidentNotificationModel> IncidentListByUserId(int userid)
    {
      using (var context = new HcDbContext())
      {
        List<IncidentNotificationModel> model = context.IncidentNotification.Where(p => p.UserID == userid).ToList();
        return model;
      }
    }


    public bool PassiveIncident(int id, bool status)
    {
      using (var context = new HcDbContext())
      {
        var result = context.IncidentNotification.Find(id);
        if (result != null)
        {
          if (status)
          {
            result.IsActive = true;
          }
          else
          {
            result.IsActive = false;
            result.IsPassiveDate = DateTime.Now;
          }
          context.SaveChanges();
          return true;
        }
        return false;
      }
    }


  }
}
