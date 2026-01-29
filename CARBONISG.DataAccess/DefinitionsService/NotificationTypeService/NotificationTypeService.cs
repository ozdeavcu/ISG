using CARBONISG.Entities;


namespace CARBONISG.DataAccess
{
  public class NotificationTypeService
  {
    public static List<NotificationTypeModel> NotificationTypeList()
    {
      using (var context = new HcDbContext())
      {
        return context.NotificationType.ToList();
      }
    }

    public static bool Save(NotificationTypeModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingNotfyType = context.NotificationType.FirstOrDefault(p => p.ID == model.ID);

            if (existingNotfyType != null)
            {
              //existingNotfyType.TypeName =  model.TypeName;
              existingNotfyType.Description = model.Description;  
              existingNotfyType.PictureUrl = !string.IsNullOrEmpty(model.PictureUrl) ? model.PictureUrl : existingNotfyType.PictureUrl;
              existingNotfyType.IsActive = model.IsActive != null ? model.IsActive : existingNotfyType.IsActive;
              existingNotfyType.UpdateAt = DateTime.Now;
            }
            else
            {
              model.CreatedAt = DateTime.Now;
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

    public static bool Delete(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var notificationmodel = context.NotificationType.FirstOrDefault(u => u.ID == id);
          if (notificationmodel != null)
          {
            context.NotificationType.Remove(notificationmodel);
            context.SaveChanges();
            return true;
          }
          else
          {
            return false;
          }
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static NotificationTypeModel GetNotificationTypeID(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          NotificationTypeModel model = context.NotificationType.FirstOrDefault(p => p.ID == id);
          if (model != null)
          {
            return model;
          }
          else
          {
            return null;
          }
        }

      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
