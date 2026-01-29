using CARBONISG.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
    public class HcDbContext:DbContext
  {
    public DbSet<UserModel> Users { get; set; }
    public DbSet<SiteFixedInformationModel> SiteFixedInformation { get; set; }
    public DbSet<LoginAttempt> LoginAttempt { get; set; }
    public DbSet<CompanyModel> Company { get; set; }
    public DbSet<DepartmantModel> Departmant { get; set; }
    public DbSet<OrderModel> Order { get; set; }
    public DbSet<TemporaryOrderModel> TemporaryOrder { get; set; }
    public DbSet<OrderCartModel> OrderCart { get; set; }
    public DbSet<InvoiceAddressModel> InvoiceAddress { get; set; }
    public DbSet<NotificationTypeModel> NotificationType { get; set; }
    public DbSet<InfringementNotificationModel> InfringementNotification { get; set; }
    public DbSet<NearMissNotificationModel> NearMissNotification { get; set; }
    public DbSet<IncidentNotificationModel> IncidentNotification { get; set; }
    public DbSet<ComplaintAndWishNotificationModel> ComplaintAndWishNotification { get; set; }
    public DbSet<NotificationPhoto> NotificationPhoto { get; set; }
    public DbSet<FaqModel> Faq { get; set; }
    public DbSet<PackagesModel> Packages { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(StringConccetion.OzdeEv);
      }
    }
    
    public static class StringConccetion
    {
      public static string Ozde = "";
      public static string OzdeEv = "";
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      SeedMasterUser(modelBuilder);
    }
    private void SeedMasterUser(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<UserModel>().HasData(new UserModel
      {
        ID = 1, 
        Username = "masteradmin",
        Name="Özde",
        Surname ="Avcu",
        Email="avcuozde8@gmail.com",
        Phone="05522721564",
        CreatedAt = DateTime.Now,
        PasswordHash = UserService.HashPassword("master123"),
        IsAdmin = AdminEnum.Master,
        IsActive=true
      });
            modelBuilder.Entity<SiteFixedInformationModel>().HasData(new SiteFixedInformationModel
            {
                ID = 1,
                LogoUrl = "/images/default-logo.png",
                Address = "Varsayılan Adres",
                ContactNumber = "111111",
                Email = "info@example.com",
                Linkedln = "https://linkedin.com/company/example",
                Twitter = "https://twitter.com/example",
                Instagram = "https://instagram.com/example",
                Facebook = "https://facebook.com/example",
                Youtube = "https://youtube.com/example"
            });
        }

    }
}
