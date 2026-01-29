
namespace CARBONISG.Entities
{
  public class PayTR_Settings
  {
    public int Id { get; set; }
    public string merchant_id { get; set; }
    public string merchant_key { get; set; }
    public string merchant_salt { get; set; }

    /// <summary>
    /// Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa
    /// Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
    /// Siparişi onaylayacağız sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
    /// </summary>
    public string merchant_ok_url { get; set; }

    /// <summary>
    /// Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa
    /// Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
    /// Siparişi iptal edeceğiniz sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
    /// </summary>
    public string merchant_fail_url { get; set; }

    /// <summary>
    /// İşlem zaman aşımı süresi - dakika cinsinden
    /// </summary>
    public string timeout_limit { get; set; }

    /// <summary>
    /// Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
    /// </summary>
    public string debug_on { get; set; }

    /// <summary>
    /// Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.
    /// </summary>
    public string test_mode { get; set; }

    /// <summary>
    /// Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın
    /// </summary>
    public string no_installment { get; set; }

    /// <summary>
    /// Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin.
    /// Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.
    /// </summary>
    public string max_installment { get; set; }

    /// <summary>
    /// Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com 
    /// üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.
    /// </summary>
    public string currency { get; set; }

    /// <summary>
    /// Türkçe için tr veya İngilizce için en gönderilebilir. Boş gönderilirse tr geçerli olur.
    /// </summary>
    public string lang { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDelete { get; set; }



    public PayTR_Settings()
    {
      CreatedDate = DateTime.Now;
      LastChangedDate = CreatedDate;
      IsDelete = false;
    }

    public static PayTR_Settings Create_HCDefaults(bool testmode)
    {
      PayTR_Settings settings = new PayTR_Settings();
      settings.merchant_id = "157585";
      settings.merchant_key = "Zwo44nzws9MZNBAq";
      settings.merchant_salt = "uAnhCmjd2acL3zuB";
      settings.timeout_limit = "30";
      settings.debug_on = testmode ? "1" : "0";
      settings.test_mode = testmode ? "1" : "0";
      settings.no_installment = "0";
      settings.max_installment = "0";
      settings.currency = "TL";
      settings.lang = "tr";

      return settings;
    }
  }
}
