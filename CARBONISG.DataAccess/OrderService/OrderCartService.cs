using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class OrderCartService
  {
    public static bool Save(OrderCartModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            model.CreateDate = DateTime.Now;
            context.Add(model);
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

    public static OrderCartModel GetOrderCartID(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          OrderCartModel model = context.OrderCart.FirstOrDefault(p => p.ID == id);
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
    public static OrderCartModel GetOrderCartByUserID(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          OrderCartModel model = context.OrderCart.FirstOrDefault(p => p.UserID == id);
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
        public static OrderCartPackageModel GetOrderCartWithPackagesByUserID(int userId)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    var result = context.OrderCart
                        .Where(orderCart => orderCart.UserID == userId)
                        .Join(
                            context.Packages,
                            orderCart => orderCart.PackageID,
                            package => package.ID,
                            (orderCart, package) => new { orderCart, package }
                        )
                        .GroupJoin(
                            context.InvoiceAddress,
                            temp => temp.orderCart.UserID,
                            invoice => invoice.UserID,
                            (temp, invoices) => new { temp, invoices }
                        )
                        .SelectMany(
                            temp => temp.invoices.DefaultIfEmpty(),
                            (temp, invoice) => new OrderCartPackageModel
                            {
                                OrderCartId = temp.temp.orderCart.ID,
                                UserID = temp.temp.orderCart.UserID,
                                PackageID = temp.temp.package.ID,
                                PackageName = temp.temp.package.PackageName,
                                PackageDescription = temp.temp.package.PackageDescription,
                                MaxEmployees = temp.temp.package.MaxEmployees,
                                IsActive = temp.temp.package.IsActive,
                                PackagePrice = temp.temp.package.Price,
                                CreateDate = temp.temp.orderCart.CreateDate,
                                PackageVAT = decimal.Round(temp.temp.package.VAT, 2),
                                TotalPrice = temp.temp.package.Price * (1 + (temp.temp.package.VAT / 100)),
                                Invoice = invoice == null ? new InvoiceAddressModel() : new InvoiceAddressModel
                                {
                                    ID = invoice.ID,
                                    SellerTaxOffice = invoice.SellerTaxOffice,
                                    BillingAddress = invoice.BillingAddress,
                                    City = invoice.City,
                                    District = invoice.District,
                                    PostalCode = invoice.PostalCode,
                                    Country = invoice.Country
                                }
                            }
                        )
                        .FirstOrDefault();

                    return result;
                }


            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool OrderCartDelete(int ordercartid)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (ordercartid > 0)
          {
            var existingOrderCart = context.OrderCart.Find(ordercartid);
            if (existingOrderCart == null)
              return false;

            context.OrderCart.Remove(existingOrderCart);

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

  }
}
