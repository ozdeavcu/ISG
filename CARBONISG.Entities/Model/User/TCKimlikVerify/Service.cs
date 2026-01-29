using ServiceReference1;


namespace CARBONISG.Entities
{
  public class UserKPSModel
  {
    public string IDNo { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Birthyear { get; set; }

    public UserKPSModel()
    {
      IDNo = "";
      Name = "";
      Surname = "";
      Birthyear = 0;
    }
    public UserKPSModel(string idNo, string pName, string pSurname, int birthYear)
    {
      IDNo = idNo;
      Name = pName;
      Surname = pSurname;
      Birthyear = birthYear;
    }
  }

  public class ServiceKPSPublic
  {
    public async Task<bool> OnGetService(UserKPSModel p)
    {
      bool result = false;
      var client = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
      var response = await client.TCKimlikNoDogrulaAsync(long.Parse(p.IDNo), p.Name, p.Surname, p.Birthyear);
      return result = response.Body.TCKimlikNoDogrulaResult;
    }
  }
}
