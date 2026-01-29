using CARBONISG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class TemporaryOrderService
  {
    public static bool AddTemporaryOrder(TemporaryOrderModel tempOrder)
    {
      try
      {
        if (tempOrder != null)
        {
          using (var context = new HcDbContext())
          {

            tempOrder.OrderDate = DateTime.Now;
            context.TemporaryOrder.Add(tempOrder);
            context.SaveChanges();
            return true;
          }
        }
        return false;
      }
      catch (Exception)
      {
        throw;
      }
    }


    public static bool UpdateTemporaryOrder(TemporaryOrderModel tempOrder)
    {
      try
      {
        if (tempOrder != null)
        {
          using (var context = new HcDbContext())
          {
            var existingOrder = context.TemporaryOrder
                                       .FirstOrDefault(o => o.GuidNumber == tempOrder.GuidNumber);

            if (existingOrder != null)
            {
              existingOrder.PaymentStatus = tempOrder.PaymentStatus;              
              existingOrder.IsActive = tempOrder.IsActive;           
              existingOrder.EndDate = DateTime.Now;      

              context.SaveChanges();
              return true;
            }
            else
            {
              return false;
            }
          }
        }
        return false;
      }
      catch (Exception)
      {
        throw;
      }
    }


    public static TemporaryOrderModel GetOrderByGuid(string merchant_oid)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if(merchant_oid !=null || merchant_oid!="")
          {
            TemporaryOrderModel result = context.TemporaryOrder.FirstOrDefault(p => p.GuidNumber == merchant_oid);
            return result;
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
