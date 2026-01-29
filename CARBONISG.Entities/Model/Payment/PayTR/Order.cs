

namespace CARBONISG.Entities
{
  public class PayTR_Order
  {
    // Müşterinizin sitenizde kayıtlı veya form vasıtasıyla aldığınız eposta adresi
    public string emailstr { get; set; }

    // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.
    public int payment_amountstr { get; set; }

    // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.
    public string merchant_oid { get; set; }

    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
    public string user_namestr { get; set; }

    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
    public string user_addressstr { get; set; }

    // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
    public string user_phonestr { get; set; }

    public string user_ip { get; set; }


    public static PayTR_Order Create_HCDefaults(uint orderID, double totalPrice, string userIP)
    {
      PayTR_Order order = new PayTR_Order();

      order.emailstr = "i@hcbilisim.com.tr";
      order.payment_amountstr = (Convert.ToInt32(totalPrice) * 100); //10 Tl için *100 yapılacak.
      order.merchant_oid = orderID.ToString();
      order.user_namestr = "Ferit";
      order.user_addressstr = "Gezgil";
      order.user_phonestr = "55555555555";
      order.user_ip = userIP;

      return order;
    }






  }
}
