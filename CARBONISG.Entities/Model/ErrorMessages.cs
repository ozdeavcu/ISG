

namespace CARBONISG.Entities
{
  public class ErrorMessages
  {
    public const string LoginUserNullError = "LOGINUSERNULL_ERROR: Kullanıcı adı ve şifre boş olamaz.";
    public const string LoginUserError = "LOGINUSER_ERROR: Kullanıcı adı veya şifre yanlış.";
    public const string DeletionError = "DELETION_ERROR: Kayıt silinirken bir hata oluştu.";
    public const string PageLoadError = "PAGELOAD_ERROR: Sayfa yüklenirken bir hata oluştu.";
    public const string RecordFoundError = "RECORDFOUND_ERROR: Kayıt bulunamadı.";
    public const string RecordError = "RECORD_ERROR: Kayıt işlemi sırasında bir hata oluştu.";
    public const string UserNameError = "USERNAME_ERROR: Kullanıcı adı veya e-posta adresi zaten kayıtlı. Lütfen kontrol edin.";
    public const string CheckPassword = "CHECK_PASSWORD: Şifreler eşleşmiyor.";

    public static string GetMessage(string errorCode)
    {
      switch (errorCode)
      {
        case LoginUserNullError:
          return LoginUserNullError.Substring(LoginUserNullError.IndexOf(":") + 2);
        case LoginUserError:
          return LoginUserError.Substring(LoginUserError.IndexOf(":") + 2);
        case DeletionError:
          return DeletionError.Substring(DeletionError.IndexOf(":") + 2);
        case PageLoadError:
          return PageLoadError.Substring(PageLoadError.IndexOf(":") + 2);
        case RecordFoundError:
          return RecordFoundError.Substring(RecordFoundError.IndexOf(":") + 2);
        case RecordError:
          return RecordError.Substring(RecordError.IndexOf(":") + 2);
        case UserNameError:
          return UserNameError.Substring(UserNameError.IndexOf(":") + 2);
        case CheckPassword:
          return CheckPassword.Substring(CheckPassword.IndexOf(":") + 2);
        default:
          return "Beklenmeyen bir hata oluştu.";
      }
    }
  }
}
