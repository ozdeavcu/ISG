using CARBONISG.Entities;


namespace CARBONISG.DataAccess
{
  public class DepartmantService
  {
    public static bool Save(DepartmantModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {

            var existingModel = context.Departmant.FirstOrDefault(p => p.ID == model.ID);
            if(existingModel == null)
            {
              context.Add(model);

            }
            else
            {
              existingModel.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : existingModel.Name;
              existingModel.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : existingModel.Description;
              existingModel.IsActive = model.IsActive != null ? model.IsActive : existingModel.IsActive;
            }
          }
          context.SaveChanges();
          return true;
        }
    
      }
      catch (Exception)
      {
        return false;
      }
    }

    public static DepartmantModel GetDepartmantID(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          DepartmantModel model = context.Departmant.FirstOrDefault(p => p.ID == id);
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

    public static List<DepartmantModel> DepartmantList()
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.Departmant.ToList();
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static List<DepartmantModel> DepartmantSelectList()
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.Departmant.Where(p=>p.IsActive).ToList();
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static bool DepartmantDelete(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var result = context.Departmant.FirstOrDefault(p => p.ID == id);
          if(result !=null)
          {
            context.Departmant.Remove(result);
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

    public static string GetDepartmentNameById(int? id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var departmentName = (from user in context.Users
                             join department in context.Departmant
                             on user.Department equals department.ID
                             where user.Department == id
                             select department.Name).FirstOrDefault();

          return departmentName;
        }
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}
