using CARBONISG.Entities;


namespace CARBONISG.DataAccess
{
    public class PackagesService
    {

        public static bool Save(PackagesModel model)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    if (model != null)
                    {
                        model.CreatedAt = DateTime.Now;
                        model.IsActive = true;
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


        public static bool Update(PackagesModel model)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    if (model != null)
                    {
                        var existingModel = context.Packages.FirstOrDefault(p => p.ID == model.ID);
                        if (existingModel != null)
                        {
                            existingModel.PackageName = model.PackageName;
                            existingModel.PackageDescription = model.PackageDescription;
                            existingModel.Price = model.Price;
                            existingModel.VAT = model.VAT;
                            existingModel.MaxEmployees = model.MaxEmployees;
                            existingModel.CreatedAt = model.CreatedAt;
                            existingModel.Update = DateTime.Now;
                            existingModel.IsActive = model.IsActive;
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

        public static List<PackagesModel> GetAllPackages()
        {
            using (var context = new HcDbContext())
            {
                return context.Packages.OrderBy(p => p.MaxEmployees).ToList();
            }

        }

        public static PackagesModel GetPackageID(int id)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    PackagesModel model = context.Packages.FirstOrDefault(p => p.ID == id);
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

        public static bool Delete(int id)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    if (id > 0)
                    {
                        var existingPackages = context.Packages.Find(id);
                        if (existingPackages == null)
                            return false;

                        context.Packages.Remove(existingPackages);

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
                return false;
            }
        }

        public static int NumberOfRemainingEmployees(int companyId, int loggedInUserId)
        {
            try
            {
                using (var context = new HcDbContext())
                {
                    var company = context.Company
             .Where(c => c.ID == companyId)
             .FirstOrDefault();

                    if (company == null)
                    {
                        return 0;
                    }

                    int? packageId = company.PackageId;

                    var package = context.Packages
                .Where(p => p.ID == packageId)
                .FirstOrDefault();
                    if (package == null)
                    {
                        return 0;
                    }
                    int packageEmployeeCount = package.MaxEmployees;

                    var currentEmployeeCount = context.Users
                        .Where(u => u.CompanyId == companyId && u.IsActive && u.ID != loggedInUserId)
                        .Count();

                    return packageEmployeeCount - currentEmployeeCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

    }
}
