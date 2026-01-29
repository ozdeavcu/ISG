using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class CompanyService
  {
    public static CompanyModel GetCompanyUserID(int userid)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.Company.FirstOrDefault(p => p.UserID == userid);
        }
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static CompanyModel GetCompanyID(int? companyid)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.Company.Find(companyid);
        }
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static bool UpdateCompany(CompanyModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var existingmodel = context.Company.Find(model.ID);
          if (existingmodel != null)
          {
            existingmodel.CompanyName= model.CompanyName;
            existingmodel.Address = model.Address;
            existingmodel.Phone = model.Phone;
            existingmodel.Email = model.Email;
            existingmodel.Tax = model.Tax;
            existingmodel.Sector = model.Sector;
            if (!string.IsNullOrEmpty(model.CompanyLogo))
            {
              existingmodel.CompanyLogo = model.CompanyLogo;
            }
            if (!string.IsNullOrEmpty(model.LogoData))
            {
              existingmodel.LogoData = model.LogoData;
            }
            existingmodel.Website = model.Website;
            existingmodel.BillingAddress = model.BillingAddress;
            existingmodel.CompanyType = model.CompanyType;
            existingmodel.Province = model.Province;
            existingmodel.District = model.District;
            existingmodel.Street = model.Street;
            if (model.PackageId!=null)
            {
              existingmodel.PackageId = model.PackageId;
            }
            existingmodel.CompanyAuthorizedName = model.CompanyAuthorizedName;
            existingmodel.CompanyAuthorizedSurname = model.CompanyAuthorizedSurname;
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

    public static string? GetLogo(int companyId)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var logo = context.Company
                            .Where(p => p.ID == companyId)
                            .Select(p => p.LogoData)
                            .FirstOrDefault(); 

          return logo; 
        }
      }
      catch (Exception)
      {
        return null; 
      }
    }


  }
}
