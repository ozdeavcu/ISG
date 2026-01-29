using CARBONISG.Entities;


namespace CARBONISG.DataAccess
{
  public class InvoiceAddressService
  {

    public static bool Save(InvoiceAddressModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            model.CreatedDate = DateTime.Now;
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


    public static bool OrderInvoiceSave(OrderCartPackageModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null && model.Invoice != null)
          {
            var existingInvoice = context.InvoiceAddress.FirstOrDefault(p => p.ID == model.Invoice.ID);

            if (existingInvoice != null)
            {
              existingInvoice.SellerTaxNumber = !string.IsNullOrEmpty(model.Invoice.SellerTaxNumber) ? model.Invoice.SellerTaxNumber : existingInvoice.SellerTaxNumber;
              existingInvoice.SellerTaxOffice = !string.IsNullOrEmpty(model.Invoice.SellerTaxOffice) ? model.Invoice.SellerTaxOffice : existingInvoice.SellerTaxOffice;
              existingInvoice.PostalCode = !string.IsNullOrEmpty(model.Invoice.PostalCode) ? model.Invoice.PostalCode : existingInvoice.PostalCode;
              existingInvoice.BillingAddress = !string.IsNullOrEmpty(model.Invoice.BillingAddress) ? model.Invoice.BillingAddress : existingInvoice.BillingAddress;
              existingInvoice.Country = !string.IsNullOrEmpty(model.Invoice.Country) ? model.Invoice.Country : existingInvoice.Country;
              existingInvoice.City = !string.IsNullOrEmpty(model.Invoice.City) ? model.Invoice.City : existingInvoice.City;
              existingInvoice.District = !string.IsNullOrEmpty(model.Invoice.District) ? model.Invoice.District : existingInvoice.District;
              existingInvoice.UpdateDate = DateTime.Now;
            }
            else
            {
              InvoiceAddressModel newInvoice = new InvoiceAddressModel
              {
                UserID = model.Invoice.UserID,
                SellerTaxOffice = model.Invoice.SellerTaxOffice,
                SellerTaxNumber = model.Invoice.SellerTaxNumber,
                BillingAddress = model.Invoice.BillingAddress,
                PostalCode = model.Invoice.PostalCode,
                Country = model.Invoice.Country,
                City = model.Invoice.City,
                District = model.Invoice.District,
                CreatedDate = DateTime.Now
              };
              context.Add(newInvoice);
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


    public static InvoiceAddressModel  GetInvoiceByUserID(int userid)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          InvoiceAddressModel model = context.InvoiceAddress.FirstOrDefault(x => x.UserID == userid);
          if(model!=null)
          {
            return model;
          }
          else
          {
            InvoiceAddressModel modelinvoice = new InvoiceAddressModel();
            return modelinvoice;
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
