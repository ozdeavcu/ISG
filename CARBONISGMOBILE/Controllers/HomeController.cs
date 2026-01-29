using CARBONISG.DataAccess;
using CARBONISG.Entities;
using CARBONISGMOBILE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CARBONISGMOBILE.Controllers
{
  public class HomeController : Controller
  {
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

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
   
      return View();
    }
    public IActionResult Pages()
    {
      return View();
    }
    public IActionResult Logout()
    {
      try
      {
        TempData.Remove("ErrorMessage");
        TempData.Remove("SuccessMessage");
        HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Login");
      }
      catch (Exception ex)
      {
        TempData["ErrorMessage"] = "Çýkýþ iþlemi sýrasýnda bir hata oluþtu. Lütfen tekrar deneyin.";
        Console.WriteLine(ex); 
        return RedirectToAction("Index", "Home"); 
      }

    }

    public IActionResult Header()
    {
      var logo = CompanyService.GetLogo(CompanyID);
      ViewData["CompanyLogoUrl"] = logo;
      return PartialView();
    }
  }

}
