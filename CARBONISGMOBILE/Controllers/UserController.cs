using CARBONISG.DataAccess;
using CARBONISG.Entities;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CARBONISGMOBILE.Controllers
{
  public class UserController : BaseController
  {
    private readonly EmailService _emailService;
    public UserController()
    {
      _emailService = new EmailService();
    }
    public IActionResult UserEdit()
    {
      try
      {
        if(UserId > 0)
        {
          UserModel model = UserService.GetUserById(UserId);
          return View(model);
        }
        else
        {
          return RedirectToAction("Index","Home");
        }     
      }
      catch (Exception)
      {
        SetErrorMessage("Bir şeyler ters gitti. Lütfen tekrar deneyin.");
        return RedirectToAction("Index", "Home");
      }
    }

    public IActionResult UserUpdate(UserModel model)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (model != null)
        {
          var result = UserService.EmployeeSave(model,false);
          if (result==0)
          {
            SetSuccessMessage("Bilgileriniz başarıyla güncellendi!");
            return RedirectToAction("Index", "Home");
          }
        }
        return View(model);
      });
    }

    public IActionResult ChangePassword()
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (UserId > 0)
        {
          var model = UserService.GetUserById(UserId);
          if (model == null)
          {
            SetErrorMessage(ErrorMessages.GetMessage(ErrorMessages.RecordFoundError));
            return View(new ChangePasswordModel());
          }
          var changePasswordModel = new ChangePasswordModel
          {
            Id = model.ID,  
          };

          return View(changePasswordModel);
        }

        return View(new ChangePasswordModel());

      });
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordModel model)
    {
      return ExecuteWithExceptionHandling(() =>
      {

      var user = UserService.GetUserById(UserId);
      if (user == null)
      {
        SetErrorMessage("Kullanıcı bulunamadı.");
        return View(model);
      }

      if (model.Password != model.ConfirmPassword)
      {
          SetErrorMessage("Şifreler uyuşmuyor.");
        return View(model);
      }

      var result = UserService.ChangePassword(model);
      if (result)
      {
        SetSuccessMessage("Şifre başarıyla değiştirildi.");
        return RedirectToAction("ChangePassword");
      }

      SetErrorMessage("Şifre değiştirilirken hata oluştu.");
      return View(model);
      });
    }

    public IActionResult ResetPassword(string token, string email, string expirationTime)
    {
      try
      {
        if (DateTime.TryParse(expirationTime, out DateTime expTime))
        {
          if (DateTime.Now > expTime)
          {
            SetErrorMessage("Bu bağlantı geçersiz veya süresi dolmuş.");
            return RedirectToAction("Login", "Home");
          }
        }
        else
        {
          SetErrorMessage("Geçersiz bağlantı.");
          return RedirectToAction("Login", "Home");
        }

        var model = new ChangePasswordModel
        {
          Token = token,
          Email = email,
          ExpirationTime = expirationTime
        };

        return View(model);
      }
      catch (Exception ex)
      {
        SetErrorMessage("Bir hata oluştu: " + ex.Message);
        return RedirectToAction("Login", "Home");
      }

    }

    [HttpPost]
    [Route("[controller]/[action]/{email}")]
    public async Task<JsonResult> SendMailCode(string email)
    {
      var result = new JsonResult("True");
      result.StatusCode = 200;
      try
      {
        Random random = new Random();
        string verificationCode = random.Next(1000, 10000).ToString();
        TempData["VerificationCode"] = verificationCode;
        string otpHtml = "";
        foreach (char c in verificationCode)
        {
          otpHtml += $"<div>{c}</div>";
        }
        var builder = new BodyBuilder();

        string htmlTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mailtemplate", "emailcodetemplate.html");

        string emailTemplate = await System.IO.File.ReadAllTextAsync(htmlTemplatePath);

        emailTemplate = emailTemplate.Replace("{{OTP_PLACEHOLDER}}", otpHtml);

        bool isOk = _emailService.SendEmail(email, "Şifre Değiştirme Talebiniz", emailTemplate);

        if (isOk)
        {
          return Json(new { success = true });
        }
        else
        {
          return Json(new { success = false });
        }

      }
      catch
      {
        return Json(new { success = false });
      }
    }



    [HttpPost]
    public IActionResult VerifyMailCode(string otpValue, string otpValue1, string otpValue2, string otpValue3)
    {
      var result = new JsonResult("True");
      result.StatusCode = 200;
      try
      {
        string mail = TempData["VerificationEmail"] as string;
        string enteredCode = otpValue + otpValue1 + otpValue2 + otpValue3;
        string systemCode = TempData["VerificationCode"]?.ToString();

        if (enteredCode == systemCode)
        {
          return result;
        }
        else
        {
          result = new JsonResult("False");
          result.StatusCode = 400;
          return result;
        }
      }
      catch
      {
        result = new JsonResult("False");
        result.StatusCode = 500;
        return result;
      }
    }

  }
}
