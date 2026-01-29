using CARBONISG.Entities;
using Microsoft.EntityFrameworkCore;

namespace CARBONISG.DataAccess
{
    public class OrderService
    {

        public static List<OrderModel> GetAllOrders()
        {
            using (var context = new HcDbContext())
            {
                var orders = context.Order
                                    .OrderByDescending(o => o.OrderDate)
                                    .ThenByDescending(o => o.OrderNumber)
                                    .ToList();

                return orders;
            }
        }

        public static List<OrderModel> GetOrdersByUserid(int userid)
        {
            using (var context = new HcDbContext())
            {
                var orders = context.Order.Where(p => p.UserID == userid)
                                    .OrderByDescending(o => o.OrderDate)
                                    .ThenByDescending(o => o.OrderNumber)
                                    .ToList();

                return orders;
            }
        }

        public static List<OrderCartPackageModel> GetOrderCartWithPackagesByUserID(int? userId = null)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    var query = from order in context.Order
                                join package in context.Packages on order.PackageID equals package.ID
                                join invoice in context.InvoiceAddress on order.UserID equals invoice.UserID into invoiceGroup
                                from invoice in invoiceGroup.DefaultIfEmpty()
                                join company in context.Company on order.UserID equals company.UserID into companyGroup
                                from company in companyGroup.DefaultIfEmpty()
                                select new OrderCartPackageModel
                                {
                                    OrderCartId = order.ID,
                                    UserID = order.UserID,
                                    PackageID = package.ID,
                                    PackageName = package.PackageName,
                                    PackageDescription = package.PackageDescription,
                                    MaxEmployees = package.MaxEmployees,
                                    IsActive = package.IsActive,
                                    PackagePrice = package.Price,
                                    CreateDate = order.OrderDate,
                                    CompanyName = company != null ? company.CompanyName : "Bilinmiyor",
                                    CustomerFullName = company != null ? $"{company.CompanyAuthorizedName} {company.CompanyAuthorizedSurname}".Trim() : "N/A",
                                    CustomerPhone = company != null ? company.Phone : "000-000-0000",
                                    PackageVAT = decimal.Round(package.VAT, 2),
                                    TotalPrice = package.Price * (1 + (package.VAT / 100)),
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
                                };

                    if (userId.HasValue)
                    {
                        query = query.Where(order => order.UserID == userId.Value);
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }



        public static OrderModel GetOrderByOrderNumber(int userid, string orderNumber)
        {
            using (var context = new HcDbContext())
            {
                var order = context.Order.Where(o => o.OrderNumber == orderNumber && o.UserID == userid).OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderNumber).FirstOrDefault();

                if (order == null)
                {
                    throw new Exception($"Order with order number {orderNumber} not found.");
                }
                return order;
            }
        }


        public OrderModel GetOrderById(int id)
        {
            using (var context = new HcDbContext())
            {
                var order = context.Order.Where(o => o.ID == id).FirstOrDefault();

                if (order == null)
                {
                    throw new Exception($"Order with ID {id} not found.");
                }
                return order;
            }
        }

        public static string GenerateRandomOrderNumber()
        {
            try
            {
                Random random = new Random();
                int minDeger = 1000000;
                int maxDeger = 9999999;
                string orderNumber = random.Next(minDeger, maxDeger).ToString();
                using (var context = new HcDbContext())
                {
                    while (context.Order.Any(o => o.OrderNumber == orderNumber) && context.TemporaryOrder.Any(o => o.OrderNumber == orderNumber))
                    {
                        orderNumber = random.Next(minDeger, maxDeger).ToString();
                    }
                }
                return orderNumber;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static bool AddOrder(OrderModel order)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    order.OrderNumber = GenerateRandomOrderNumber();
                    order.OrderDate = DateTime.Now;
                    context.Order.Add(order);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error while saving order: " + ex.Message);
            }
        }

        public static bool UpdateOrder(OrderModel order)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    context.Order.Update(order);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error while updating order: " + ex.Message);
            }
        }

        public bool CreateOrder(TemporaryOrderModel orderModel)
        {
            try
            {
                if (orderModel == null)
                {
                    return false;
                }
                var order = new OrderModel
                {
                    OrderNumber = orderModel.OrderNumber,
                    UserID = orderModel.UserID,
                    CompanyID = orderModel.CompanyID,
                    PackageID = orderModel.PackageID,
                    InvoiceID = orderModel.InvoiceID,
                    OrderDate = orderModel.OrderDate,
                    TotalAmount = orderModel.TotalAmount,
                    UserIP = orderModel.UserIP,
                    PaymentStatus = orderModel.PaymentStatus,
                    IsActive = true
                };

                bool result = AddOrder(order);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving order: " + ex.Message);
            }
        }


        public static OrderCartPackageModel GetOrderWithPackageById(int orderid)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    var result = (from order in context.Order
                                  where order.ID == orderid
                                  join package in context.Packages on order.PackageID equals package.ID
                                  join invoice in context.InvoiceAddress on order.UserID equals invoice.UserID into invoiceGroup
                                  from invoice in invoiceGroup.DefaultIfEmpty()
                                  join company in context.Company on order.UserID equals company.UserID into companyGroup
                                  from company in companyGroup.DefaultIfEmpty()
                                  select new OrderCartPackageModel
                                  {
                                      OrderCartId = order.ID,
                                      OrderNumber=order.OrderNumber,
                                      UserID = order.UserID,
                                      PackageID = package.ID,
                                      PackageName = package.PackageName,
                                      PackageDescription = package.PackageDescription,
                                      MaxEmployees = package.MaxEmployees,
                                      IsActive = package.IsActive,
                                      PackagePrice = package.Price,
                                      CreateDate = order.OrderDate,
                                      PackageVAT = decimal.Round(package.VAT, 2),
                                      TotalPrice = package.Price * (1 + (package.VAT / 100)),
                                      CustomerFullName = company != null ? $"{company.CompanyAuthorizedName} {company.CompanyAuthorizedSurname}".Trim() : "N/A",
                                      CustomerPhone = company != null ? company.Phone : "000-000-0000",
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
                                  }).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }


    }
}
