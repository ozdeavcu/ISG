using CARBONISG.DataAccess;
using CARBONISG.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CARBONISGMOBILE.Controllers
{
  public class LoginController : Controller
  {
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LoginControl([FromForm] LoginViewModel model)
    {
      try
      {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        if (model.Password == null || model.LoginIdentifier == null)
        {
          ViewBag.ErrorMessage = ErrorMessages.GetMessage(ErrorMessages.LoginUserNullError);
          return RedirectToAction("Login", "Login", model);
        }

        UserService userService = new UserService();
        UserModel user = userService.AuthenticateUser(model.LoginIdentifier, model.Password, ipAddress,"mobile");

        if (user == null)
        {
          ViewBag.ErrorMessage = ErrorMessages.GetMessage(ErrorMessages.LoginUserError);
          return RedirectToAction("Login", "Login", model);
        }

        var claims = new List<Claim>
        {
        new Claim("ID", user.ID.ToString()),
        new Claim("CompanyID", user.CompanyId.ToString()),
        new Claim("JobTitle", user.JobTitle.ToString()),
        new Claim(ClaimTypes.Name, (user.Name + ' ' + user.Surname)),
        new Claim(ClaimTypes.Role, Enum.GetName(typeof(AdminEnum), user.IsAdmin))
        };

        if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
        {
          claims.Add(new Claim("Image", user.ProfilePictureUrl));
        }

        var userIdentity = new ClaimsIdentity(claims, "login");
        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
        HttpContext.SignInAsync(principal);
        return RedirectToAction("Index", "Home");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Giriş Yapmaya Çalışırken Hata!: {ex.Message}");
        return RedirectToAction("Login", "Login");
      }
    }




  }
}
