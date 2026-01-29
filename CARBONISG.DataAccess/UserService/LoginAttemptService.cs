using CARBONISG.Entities;
using Microsoft.AspNetCore.Http;

namespace CARBONISG.DataAccess
{
  public class LoginAttemptService
  {
    private readonly HcDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginAttemptService(HcDbContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }
    public LoginAttemptService(HcDbContext context)
    {
      _context = context;
    }
    public void LogLoginAttempt(UserModel user, bool isSuccess, string ipAddress)
    {

      var loginAttempt = new LoginAttempt
      {
        UserId = user?.ID ?? 0,
        Username = user?.Username ?? "Unknown",
        AttemptTime = DateTime.Now,
        IPAddress = ipAddress,
        IsSuccess = isSuccess
      };

      _context.LoginAttempt.Add(loginAttempt);
      _context.SaveChanges();

    }
    public List<LoginAttempt> GetLoginAttemptsForUser(UserModel user)
    {
      return _context.LoginAttempt.Where(attempt => attempt.UserId == user.ID).ToList();

    }
    public static List<LoginAttempt> GetAllLoginAttempts()
    {
      using (var context = new HcDbContext())
      {
        return context.LoginAttempt
                   .OrderByDescending(attempt => attempt.AttemptTime)
                   .ToList();
      }
     
    }
  }
}
