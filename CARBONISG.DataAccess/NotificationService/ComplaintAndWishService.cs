using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class ComplaintAndWishService
  {
    public List<ComplaintAndWishNotificationModel> ComplaintAndWishList()
    {
      using (var context = new HcDbContext())
      {
        return context.ComplaintAndWishNotification.ToList();
      }
    }

    public bool Save(ComplaintAndWishNotificationModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingModel = context.ComplaintAndWishNotification.FirstOrDefault(p => p.ID == model.ID);

            if (existingModel != null)
            {
              existingModel.UserID = model.UserID;
              existingModel.ReportedAt = model.ReportedAt;
              existingModel.ReportedAtTime = model.ReportedAtTime;
              existingModel.IsNameVisible = model.IsNameVisible;
              existingModel.IsActive = model.IsActive;
              existingModel.ComplaintOrWishDescription = model.ComplaintOrWishDescription;
              existingModel.Suggestions = model.Suggestions;
              existingModel.ComplaintOrWish = model.ComplaintOrWish;
              existingModel.AudioUrl = model.AudioUrl;
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

    public ComplaintAndWishNotificationModel GetComplaintAndWishById(int id)
    {
      using (var context = new HcDbContext())
      {
        ComplaintAndWishNotificationModel model = context.ComplaintAndWishNotification.Find(id);
        return model;
      }
    }

    public List<ComplaintAndWishNotificationModel> ComplaintAndWishListByUserId(int userid)
    {
      using (var context = new HcDbContext())
      {
        List<ComplaintAndWishNotificationModel> model= context.ComplaintAndWishNotification.Where(p=>p.UserID==userid).ToList();
        return model;
      }
    }

    public bool PassiveComplaintAndWish(int id, bool status)
    {
      using (var context = new HcDbContext())
      {
        var result = context.ComplaintAndWishNotification.Find(id);
        if(result != null)
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
