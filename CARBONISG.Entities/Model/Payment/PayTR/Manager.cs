using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace CARBONISG.Entities
{
  public class Manager
  {
    public ResultIframe GetIframe(PayTR_Settings setting, PayTR_Order order, OrderCartPackageModel basket)
    {
      ResultIframe iframeResult = new ResultIframe();

      //.Prepare_Basket
      string user_basket_json = JsonSerializer.Serialize(basket);
      string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));

      //.Prepare_Token - değiştirilmeden kullanılmalıdır.
      string Birlestir = string.Concat(setting.merchant_id, order.user_ip, order.merchant_oid, order.emailstr, order.payment_amountstr.ToString(), user_basketstr, setting.no_installment, setting.max_installment, setting.currency, setting.test_mode, setting.merchant_salt);
      HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(setting.merchant_key));
      byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));


      //.Construct Data
      NameValueCollection data = new NameValueCollection();
      data["merchant_id"] = setting.merchant_id;
      data["user_ip"] = order.user_ip;
      data["merchant_oid"] = order.merchant_oid;
      data["email"] = order.emailstr;
      data["payment_amount"] = order.payment_amountstr.ToString();
      data["user_basket"] = user_basketstr;
      data["paytr_token"] = Convert.ToBase64String(b);
      data["debug_on"] = setting.debug_on;
      data["test_mode"] = setting.test_mode;
      data["no_installment"] = setting.no_installment;
      data["max_installment"] = setting.max_installment;
      data["user_name"] = order.user_namestr;
      data["user_address"] = order.user_addressstr;
      data["user_phone"] = order.user_phonestr;
      data["merchant_ok_url"] = setting.merchant_ok_url;
      data["merchant_fail_url"] = setting.merchant_fail_url;
      data["timeout_limit"] = setting.timeout_limit;
      data["currency"] = setting.currency;
      data["lang"] = setting.lang;

      //.Connect
      using (WebClient client = new WebClient())
      {
        client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
        string ResultAuthTicket = Encoding.UTF8.GetString(result);
        dynamic json = JToken.Parse(ResultAuthTicket);

        if (json.status == "success")
        {
          iframeResult.IsSuccess = true;
          iframeResult.ErrorMessage = "";
          iframeResult.IframeUrl = "https://www.paytr.com/odeme/guvenli/" + json.token;
        }
        else
        {
          iframeResult.IsSuccess = false;
          iframeResult.ErrorMessage = "PAYTR IFRAME failed. reason:" + json.reason + "";
          iframeResult.IframeUrl = "";
        }
      }

      return iframeResult;
    }
  }
}
