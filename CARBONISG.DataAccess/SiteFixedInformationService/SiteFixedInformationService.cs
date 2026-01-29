using CARBONISG.Entities;


namespace CARBONISG.DataAccess
{
  public class SiteFixedInformationService
  {
    public static List<SiteFixedInformationModel> FixedInfoList()
    {
      try
      {
        using (var context = new HcDbContext())
        {
          return context.SiteFixedInformation.ToList();
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static SiteFixedInformationModel FixedInfoGetRecord(int id=0)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          SiteFixedInformationModel existingInfo = null;
          if (id>0)
          {
            existingInfo = context.SiteFixedInformation.FirstOrDefault(p => p.ID == id);
          }
          else
          {
            existingInfo = context.SiteFixedInformation.FirstOrDefault();
          }
          return existingInfo;
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static int FixedInfoSave()
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var existingmodel = context.SiteFixedInformation.FirstOrDefault();
          if (existingmodel == null)
          {
            SiteFixedInformationModel model = new SiteFixedInformationModel();
            model.UpdateDate = DateTime.Now;
            context.Add(model);
            context.SaveChanges();
            return model.ID;
          }
          else
          {
            return existingmodel.ID;
          }
      
        }
      }
      catch (Exception)
      {
        return -1;
      }
    }

    public static bool FixedInfoUpdate(SiteFixedInformationModel model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existinginfo= context.SiteFixedInformation.FirstOrDefault(p=>p.ID == model.ID);
            existinginfo.UpdateDate = DateTime.Now;
            existinginfo.LogoUrl = string.IsNullOrEmpty(model.LogoUrl) ? existinginfo.LogoUrl : model.LogoUrl;
            existinginfo.Address = model.Address;
            existinginfo.ContactNumber = model.ContactNumber;
            existinginfo.Email = model.Email;
            existinginfo.LinkedlnIcon = string.IsNullOrEmpty(model.LinkedlnIcon) ? existinginfo.LinkedlnIcon : model.LinkedlnIcon;
            existinginfo.Linkedln = model.Linkedln;
            existinginfo.TwitterIcon = string.IsNullOrEmpty(model.TwitterIcon) ? existinginfo.TwitterIcon : model.TwitterIcon;
            existinginfo.Twitter = model.Twitter;
            existinginfo.InstagramIcon = string.IsNullOrEmpty(model.InstagramIcon) ? existinginfo.InstagramIcon : model.InstagramIcon;
            existinginfo.Instagram = model.Instagram;
            existinginfo.FacebookIcon = string.IsNullOrEmpty(model.FacebookIcon) ? existinginfo.FacebookIcon : model.FacebookIcon;
            existinginfo.Facebook = model.Facebook;
            existinginfo.YoutubeIcon = string.IsNullOrEmpty(model.YoutubeIcon) ? existinginfo.YoutubeIcon : model.YoutubeIcon;
            existinginfo.Youtube = model.Youtube;
            existinginfo.WhatsappIcon = string.IsNullOrEmpty(model.WhatsappIcon) ? existinginfo.WhatsappIcon : model.WhatsappIcon;
            existinginfo.WhatsappPhone = model.WhatsappPhone;
          }
          context.SaveChanges();
          return true;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}
