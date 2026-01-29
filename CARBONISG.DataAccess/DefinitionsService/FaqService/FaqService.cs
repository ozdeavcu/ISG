using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class FaqService
  {
    public static FaqModel GetFaqById(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          FaqModel? model = context.Faq.Find(id);
          if (model != null)
          {
            return model;
          }
          else
          {
            FaqModel faqmodel = new FaqModel();
            return faqmodel;  
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
      }

    }

    public static bool Save(FaqModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if(model!=null)
          {
            var existingmodel = context.Faq.FirstOrDefault(p => p.ID == model.ID);
            if(existingmodel ==null)
            {
              model.CreationDate = DateTime.Now;
              context.Faq.Add(model);
            }
            else
            {
              existingmodel.Title = model.Title;
              existingmodel.Description = model.Description;
              existingmodel.UpdateDate = DateTime.Now;
              existingmodel.IsActive=model.IsActive;
              existingmodel.Ranking=model.Ranking;
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

    public static List<FaqModel> FaqList()
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.Faq.OrderBy(p=>p.Ranking).ToList();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
      }
    }

    public static bool FaqDelete(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var result = context.Faq.FirstOrDefault(p => p.ID == id);
          if (result != null)
          {
            context.Faq.Remove(result);
            context.SaveChanges();
            return true;
          }
          else
          {
            return false;
          }

        }

      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
      }

    }
  }
}
