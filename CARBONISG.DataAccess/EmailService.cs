using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class EmailService
  {
    private readonly string smtpServer = "smtp.gmail.com";
    private readonly int smtpPort = 587;
    private readonly string smtpUser = "support@hcbilisim.com.tr";
    private readonly string smtpPassword = "Hc128256?";
    private readonly string fromEmail = "support@hcbilisim.com.tr";
    private readonly string displayName = "HC Bilişim Destek";

    public bool SendEmail(string recipientEmail, string subject, string body)
    {
      try
      {
        using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
        {
          smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
          smtpClient.EnableSsl = true;

          var mailMessage = new MailMessage
          {
            From = new MailAddress(fromEmail, displayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
          };

          mailMessage.To.Add(recipientEmail);
          smtpClient.Send(mailMessage);
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"E-posta gönderimi başarısız: {ex.Message}");
        return false;
      }
    }
  }
}
