using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class NearMissService
  {
    public List<NearMissNotificationModel> NearMissList()
    {
      using (var context = new HcDbContext())
      {
        return context.NearMissNotification.ToList();
      }
    }

    public bool Save(NearMissNotificationModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingModel = context.NearMissNotification.FirstOrDefault(p => p.ID == model.ID);

            if (existingModel != null)
            {
              existingModel.UserID = model.UserID;
              existingModel.ReportedAt = model.ReportedAt;
              existingModel.ReportedAtTime = model.ReportedAtTime;
              existingModel.IsNameVisible = model.IsNameVisible;
              existingModel.IsActive = model.IsActive;
              existingModel.NearMissLocation = model.NearMissLocation;
              existingModel.NearMissDate = model.NearMissDate;
              existingModel.NearMissTime = model.NearMissTime;
              existingModel.NearMissDescription = model.NearMissDescription;
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

    public NearMissNotificationModel GetNearMissById(int id)
    {
      using (var context = new HcDbContext())
      {
        NearMissNotificationModel model = context.NearMissNotification.Find(id);
        return model;
      }
    }

    public List<NearMissNotificationModel> NearMissListByUserId(int userid)
    {
      using (var context = new HcDbContext())
      {
        List<NearMissNotificationModel> model = context.NearMissNotification.Where(p => p.UserID == userid).ToList();
        return model;
      }
    }

    public bool PassiveNearMiss(int id,bool status)
    {
      using (var context = new HcDbContext())
      {
        var result = context.NearMissNotification.Find(id);
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
