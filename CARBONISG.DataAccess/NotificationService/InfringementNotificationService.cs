using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class InfringementNotificationService
  {
    public List<InfringementNotificationModel> InfringementNotificationList()
    {
      using (var context = new HcDbContext())
      {
        return context.InfringementNotification.ToList();
      }
    }

    public bool Save(InfringementNotificationModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingModel = context.InfringementNotification.FirstOrDefault(p => p.ID == model.ID);

            if (existingModel != null)
            {
              existingModel.UserID = model.UserID;
              existingModel.ReportedAt = model.ReportedAt;
              existingModel.ReportedAtTime = model.ReportedAtTime;
              existingModel.IsNameVisible = model.IsNameVisible;
              existingModel.IsActive = model.IsActive;
              existingModel.InfringementLocation = model.InfringementLocation;
              existingModel.InfringementDescription=model.InfringementDescription;
              existingModel.InfringementSection = model.InfringementSection;
              existingModel.InfringementSolution = model.InfringementSolution;
              if (model.AudioData != null && model.AudioData.Length > 0)
              {
                existingModel.AudioData = model.AudioData;
              }
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

    public InfringementNotificationModel GetInfringementById(int id)
    {
      using (var context = new HcDbContext())
      {
        InfringementNotificationModel model= context.InfringementNotification.Find(id);
        return model;
      }
    }

    public List<InfringementNotificationModel> InfringementListByUserId(int userid)
    {
      using (var context = new HcDbContext())
      {
        List<InfringementNotificationModel> model = context.InfringementNotification.Where(p => p.UserID == userid).ToList();
        return model;
      }
    }

    public bool PassiveInfringement(int id,bool status)
    {
      using (var context = new HcDbContext())
      {
        var result = context.InfringementNotification.Find(id);
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
