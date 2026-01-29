using CARBONISG.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text;


namespace CARBONISG.DataAccess
{
  public class UserService
  {
    //Çalışan kaydı
    public static int EmployeeSave(UserModel model,bool isNewUser)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (isNewUser)
          {
            var UserWithSameUsernameOrEmail = context.Users.FirstOrDefault(u => u.Email == model.Email && u.ID != model.ID);
            if (UserWithSameUsernameOrEmail != null)
            {
              return -4;
            }
            if (model.PasswordHash != null) {
              model.HashPassword();
            }
         
            model.CreatedAt = DateTime.Now;
            model.IsActive = true;
            model.IsAdmin = AdminEnum.Calisan;
            context.Users.Add(model);
          }
          else
          {
            var existingUser = context.Users.FirstOrDefault(u => u.ID == model.ID);
            if (existingUser != null)
            {
              var isEmailInUse = context.Users.Any(u => u.Email == model.Email && u.ID != model.ID);
              if (isEmailInUse)
              {
                return -4;
              }
              if (model.PasswordHash != null)
              {
                existingUser.PasswordHash = model.PasswordHash;
                existingUser.ConfirmPassword = model.PasswordHash;
              }
              existingUser.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : existingUser.Name;
              existingUser.Surname = !string.IsNullOrEmpty(model.Surname) ? model.Surname : existingUser.Surname;
              existingUser.Email = !string.IsNullOrEmpty(model.Email) ? model.Email : existingUser.Email;
              existingUser.Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : existingUser.Phone;
              existingUser.RoleId = model.RoleId != null ? model.RoleId : existingUser.RoleId;
              existingUser.CompanyId = model.CompanyId != null ? model.CompanyId : existingUser.CompanyId;
              existingUser.CreatedAt = DateTime.Now;
              existingUser.IsActive = model.IsActive != null ? model.IsActive : existingUser.IsActive;
              existingUser.DateLeft = model.DateLeft ?? null;
              if (model.DateLeft != null)
              {
                existingUser.IsActive = false;
              }
              existingUser.DateOfBirth = model.DateOfBirth ?? existingUser.DateOfBirth;
              existingUser.ProfilePictureUrl = !string.IsNullOrEmpty(model.ProfilePictureUrl) ? model.ProfilePictureUrl : existingUser.ProfilePictureUrl;
              existingUser.JobTitle = !string.IsNullOrEmpty(model.JobTitle) ? model.JobTitle : existingUser.JobTitle;
              existingUser.DateHired = model.DateHired;
              existingUser.EmergencyContactName = !string.IsNullOrEmpty(model.EmergencyContactName) ? model.EmergencyContactName : existingUser.EmergencyContactName;
              existingUser.EmergencyContactPhone = !string.IsNullOrEmpty(model.EmergencyContactPhone) ? model.EmergencyContactPhone : existingUser.EmergencyContactPhone;
              existingUser.Department = model.Department != null ? model.Department : existingUser.Department;
              existingUser.IdentityNumber = !string.IsNullOrEmpty(model.IdentityNumber) ? model.IdentityNumber : existingUser.IdentityNumber;
              existingUser.Address = !string.IsNullOrEmpty(model.Address) ? model.Address : existingUser.Address;
            }
            else
            {
              return -1;
            }
          }
          context.SaveChanges();
          return 0;
        }

      }
      catch (Exception)
      {

        return -2;
      }
    }

    //Yönetici ve şirket kaydı
    public static int SaveRegister(RegisterModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var UserWithSameUsernameOrEmail = context.Users
          .FirstOrDefault(u => (u.Username.ToLower() == model.UserName.ToLower() || u.Email.ToLower() == model.Email.ToLower()));
          if (UserWithSameUsernameOrEmail != null)
          {
            return -4; //bu kullanıcı adı veya mail ile kullanıcı kayıtlı
          }
          if(model.Password != model.RePassword)
          {
            return -4; //şifreler aynı değil
          }
          model.HashPassword();
          UserModel userModel = new UserModel
          {
            PasswordHash = model.Password,
            Email = model.Email,
            Username = model.UserName,
            Name = model.CompanyAuthorizedName,
            Surname = model.CompanyAuthorizedSurname,
            IsAdmin = AdminEnum.Yonetici,
            CreatedAt = DateTime.Now,
            IsActive = true,
            Phone = model.Phone
          };

          context.Users.Add(userModel);
          context.SaveChanges();
          CompanyModel companyModel = new CompanyModel
          {
            CompanyName = model.CompanyName,
            CompanyAuthorizedName = model.CompanyAuthorizedName,
            CompanyAuthorizedSurname = model.CompanyAuthorizedSurname,
            Phone = model.Phone,
            Address = model.Address,
            Email = model.Email,
            CreatedAt = DateTime.Now,
            IsActive = true,
            PackageId = 1,
            Province = model.Province,
            District = model.District,
            Street = model.Street,
            UserID = userModel.ID
          };

          context.Company.Add(companyModel);
          context.SaveChanges();
          userModel.CompanyId = companyModel.ID;
          context.Users.Update(userModel);
          context.SaveChanges();
          return 0;
        }
      }
      catch (Exception)
      {
        return -1;
      }
    }

    public int SaveProfilePhoto(int userid, IFormFile formFile)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (formFile != null)
          {
            var extent = Path.GetExtension(formFile.FileName);
            var randomName = ($"{Guid.NewGuid()}{extent}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
              formFile.CopyTo(stream);
            }
            var user = context.Users.FirstOrDefault(x => x.ID == userid);
            if (user != null)
            {
              user.ProfilePictureUrl = randomName;
              context.SaveChanges();
              return 1;//başarılı
            }
            else
            {
              return 0;//kullanıcı bulunamadı
            }
          }
          return 0;//dosya yoksa
        }

      
      }
      catch
      {

        return -1;
      }

    }

    public int DeleteUser(int userId)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var user = context.Users.FirstOrDefault(u => u.ID == userId);
          if (user != null)
          {
            context.Users.Remove(user);
            context.SaveChanges();
            return 0;
          }
          else
          {
            return -1;
          }
        }

        
      }
      catch (Exception)
      {
        return -2;
      }
    }

    public static List<UserModel> GetAllUsers()
    {
      using (var context = new HcDbContext())
      {
        return context.Users.Where(p=>p.IsAdmin != AdminEnum.Master).ToList();
      }
    }

    public static List<UserModel> GetEmployeeList(int id)
    {
      using (var context = new HcDbContext())
      {
        var company = context.Users
       .Where(p => p.ID == id && p.IsAdmin == AdminEnum.Yonetici && p.CreatedAt != null) 
       .OrderByDescending(p => p.CreatedAt)
       .FirstOrDefault();

        int? companyid = 0;
        if (company != null)
        {
          companyid = company.CompanyId;

        }
        return context.Users.Where(p=>p.CompanyId == companyid && p.ID!=id).ToList();
      }
    }


    public static UserModel GetUserById(int userId)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          UserModel user = context.Users.FirstOrDefault(u => u.ID == userId);
          if (user != null)
          {
            return user;
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

    public static int GetEmailUser(string email)
    {
      try
      {
        using (var context = new HcDbContext())
        {

          UserModel user = GetAllUsers().FirstOrDefault(u => u.Email.Equals(email, StringComparison.Ordinal));

          if (user != null)
          {
            return user.ID;
          }
          else
          {
            return -1;
          }
        }
     
      }
      catch (Exception ex)
      {
        return -1;
      }
    }
    public static string HashPassword(string password)
    {
      using (SHA256 sha256Hash = SHA256.Create())
      {
        byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in hashBytes)
        {
          builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
      }
    }

    private bool VerifyPassword(string password, string storedHash)
    {
      if (string.IsNullOrWhiteSpace(password))
        return false;

      string hashedPassword = HashPassword(password);
      return hashedPassword == storedHash;
    }

    public UserModel AuthenticateUser(string loginIdentifier, string password,string ipAddress,string path)
    {
      using (var context = new HcDbContext())
      {
        UserModel? user;
        if (path=="web")
        {
           user = context.Users.FirstOrDefault(u => u.Username == loginIdentifier || u.Email == loginIdentifier && u.IsActive == true);
        }
        else if(path=="mobile")
        {
          user = context.Users.FirstOrDefault(u => u.Email == loginIdentifier && u.IsAdmin==AdminEnum.Calisan && u.IsActive==true);
        }
        else
        {
          return null;
        }

        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
          LogLoginAttempt(user, false, ipAddress);
          return null;
        }

        LogLoginAttempt(user, true,ipAddress);
        return user;
      }
    }

    private void LogLoginAttempt(UserModel user, bool success,string ipAddress)
    {
      using (var loginAttemptContext = new HcDbContext())
      {
        LoginAttemptService loginAttemptService = new LoginAttemptService(loginAttemptContext);
        loginAttemptService.LogLoginAttempt(user, success,ipAddress);
      }

    }

    public static string GetCompanyNameByUserId(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var companyName = (from user in context.Users
                             join company in context.Company
                             on user.CompanyId equals company.ID
                             where user.ID == id
                             select company.CompanyName).FirstOrDefault();

          return companyName;
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static bool MaxEmployee(int userid)
        {
            try
            {
                if (userid > 0)
                {
                    using (var context = new HcDbContext())
                    {
                        var result = context.Company.FirstOrDefault(u => u.UserID == userid);
                        if (result != null)
                        {
                            var package = context.Packages.FirstOrDefault(p => p.ID == result.PackageId);
                            var employeecount = context.Users.Count(p => p.CompanyId == result.ID && p.ID != userid);
                            
                            if (package!=null && package.MaxEmployees > employeecount)
                            {
                                return true;
                            }
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

    public static bool CheckPassword(string password)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if(password!="" || password!=null)
          {
            var existingpassword = context.Users.Where(p=>p.PasswordHash == password).FirstOrDefault();
            if (existingpassword != null) {
              return true;
            }
            return false;
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

    public static string CreatePassword()
    {
      const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
      const string numbers = "0123456789";
      const string specialChars = "!.";

      Random random = new Random();

      string password = upperCase[random.Next(upperCase.Length)].ToString() +
                        lowerCase[random.Next(lowerCase.Length)].ToString() +
                        numbers[random.Next(numbers.Length)].ToString() +
                        specialChars[random.Next(specialChars.Length)].ToString();

      string allChars = upperCase + lowerCase + numbers + specialChars;

      for (int i = 4; i < 8; i++)
      {
        password += allChars[random.Next(allChars.Length)];
      }

      return new string(password.ToCharArray().OrderBy(c => random.Next()).ToArray());
    }

    public static string GenerateAndSavePassword(string email)
    {
      using (var context = new HcDbContext())
      {
        string newPassword;

        do
        {
          newPassword = CreatePassword();

          string hashedPassword = HashPassword(newPassword);

          bool exists = context.Users.Any(p => p.PasswordHash == hashedPassword);
          if (!exists)
          {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
              user.PasswordHash = hashedPassword;
              context.SaveChanges();
            }
            else
            {
              throw new Exception("Kullanıcı bulunamadı.");
            }

            return newPassword;
          }
        } while (true);
      }
    }

    public static int VerifyID(string IDNo, string pName, string pSurname, int birthYear)
    {
      try
      {
        ServiceKPSPublic service = new ServiceKPSPublic();
        UserKPSModel usr = new UserKPSModel(IDNo, pName, pSurname, birthYear);
        var result = service.OnGetService(usr);

        return (result == null) ? 99 :
          ((result.Result) ? 0 : 1);
      }
      catch
      {
        return -1;
      }
    }
    public static string GetUserFullName(int userid)
    {
      using (var context = new HcDbContext())
      {
        var fullName = context.Users
                              .Where(p => p.ID == userid)
                              .Select(p => p.Name + " " + p.Surname)
                              .FirstOrDefault();

        return fullName ?? "Kullanıcı bulunamadı";
      }
    }

    public static int GetTotalUser(int? companyId, bool isMaster)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          int totalUser = 0;

          if (isMaster)
          {
            totalUser = context.Users.Count();
          }
          else if (companyId.HasValue)
          {
            totalUser = context.Users.Count(n => n.CompanyId == companyId);
          }

          return totalUser;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Bildirim sayısı alınırken hata oluştu.", ex);
      }
    }

    public static bool ChangePassword(ChangePasswordModel model)
    {
      using (var context = new HcDbContext())
      {
        var user = context.Users.FirstOrDefault(u => u.ID == model.Id);
        if (user == null)
        {
          return false;
        }

        user.PasswordHash = HashPassword(model.Password);
        context.SaveChanges();
        return true;
      } 
    }




  }
}
