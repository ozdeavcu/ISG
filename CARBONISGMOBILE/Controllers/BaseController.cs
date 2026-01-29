using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CARBONISGMOBILE.Controllers
{
  public class BaseController : Controller
  {
    protected int UserId
    {
      get
      {
        try
        {
          var userIdString = HttpContext.User.FindFirst("Id")?.Value;
          return string.IsNullOrEmpty(userIdString) ? 0 : int.Parse(userIdString);
        }
        catch
        {
          return 0;
        }
      }
    }
    protected int CompanyID
    {
      get
      {
        try
        {
          var userIdString = HttpContext.User.FindFirst("CompanyID")?.Value;
          return string.IsNullOrEmpty(userIdString) ? 0 : int.Parse(userIdString);
        }
        catch
        {
          return 0;
        }
      }
    }


    protected IActionResult ExecuteWithExceptionHandling(Func<IActionResult> action)
    {
      try
      {
        return action();
      }
      catch (ValidationException ex)
      {
        SetErrorMessage($"Doğrulama hatası: {ex.Message}");
        return View();
      }
      catch (DbUpdateException ex)
      {
        SetErrorMessage($"Veritabanı işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.");
        return RedirectToAction("Index", "Home");
      }
      catch (Exception ex)
      {
        SetErrorMessage($"Bir şeyler ters gitti. Lütfen tekrar deneyin.");
        return RedirectToAction("Index", "Home");
      }
    }

    protected void SetSuccessMessage(string message)
    {
      TempData["SuccessMessage"] = message;
    }

    protected void SetErrorMessage(string message)
    {
         ViewBag.ErrorMessage = message;
    }

    protected void SetInfoMessage(string message)
    {
      TempData["InfoMessage"] = message;
    }

    protected string GetMessage(string key)
    {
      return TempData[key]?.ToString() ?? string.Empty;
    }
  }
}
