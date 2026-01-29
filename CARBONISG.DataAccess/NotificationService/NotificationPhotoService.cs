using CARBONISG.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARBONISG.DataAccess
{
  public class NotificationPhotoService
  {
    public static bool Save(NotificationPhoto model)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          if (model != null)
          {
            var existingModel = context.NotificationPhoto
                .FirstOrDefault(p => p.PhotoUrl == model.PhotoUrl
                                     && p.NotificationID == model.NotificationID
                                     && p.UserID == model.UserID);

            if (existingModel != null)
            {
              existingModel.UpdateDate = DateTime.Now;
              existingModel.Type = model.Type;
            }
            else
            {
              model.CreateDate = DateTime.Now;
              context.Add(model);
            }

            context.SaveChanges();
            return true;
          }

          return false;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"NotificationPhoto kaydedilirken hata: {ex.Message}");
        return false;
      }
    }


    //private static readonly HttpClient client = new HttpClient();

    //private static async Task<bool> UploadToWebAPI(IFormFile file, string fileName)
    //{
    //  try
    //  {
    //    var apiUrl = "http://localhost:5228/api/images";


    //    using (var memoryStream = new MemoryStream())
    //    {
    //      await file.CopyToAsync(memoryStream);
    //      memoryStream.Position = 0;

    //      var content = new MultipartFormDataContent();
    //      var fileContent = new StreamContent(memoryStream);
    //      fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

    //      content.Add(fileContent, "file", fileName);

    //      var response = await client.PostAsync(apiUrl, content);
    //      var responseContent = await response.Content.ReadAsStringAsync();
    //      Console.WriteLine($"Response Status Code: {response.StatusCode}");
    //      Console.WriteLine($"Response Content: {responseContent}");

    //      return response.IsSuccessStatusCode;
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Console.WriteLine($"Web API'ye yükleme sırasında hata: {ex.Message}");
    //    return false;
    //  }
    //}


    public static async Task SaveImages(List<IFormFile> multiImages, int notificationId, int userId,int companyID)
    {
      if (multiImages != null && multiImages.Count > 0)
      {
        foreach (var file in multiImages)
        {
          var fileName = Path.GetFileName(file.FileName);
          string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            file.CopyTo(stream);
          }

          //var apiResponse = await UploadToWebAPI(file, fileName);

          //if (!apiResponse)
          //{
          //  Console.WriteLine($"Dosya {fileName} web API'ye yüklenirken hata oluştu.");
          //  continue;
          //}
          string photoData = null;
          using (var memoryStream = new MemoryStream())
          {
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            photoData = Convert.ToBase64String(fileBytes); 
          }

          var notificationPhoto = new NotificationPhoto
          {
            CompanyID = companyID,
            NotificationID = notificationId,
            UserID = userId,
            PhotoUrl = $"/uploads/{fileName}",
            PhotoData = photoData, 
            Type = NotificationType.Infringement
          };

          Save(notificationPhoto);
        }
      }
    }


    public static List<NotificationPhoto> GetNotificationIDType(int userid,int notificationID,NotificationType type)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          List<NotificationPhoto> existingPhoto = context.NotificationPhoto.Where(p=>p.NotificationID == notificationID && p.Type==type && p.UserID==userid).ToList();
          return existingPhoto;
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine($"NotificationPhoto bilgisi alınırken hata: {ex.Message}");
        List<NotificationPhoto> photo = new List<NotificationPhoto>();
        return photo;
      }
    }

    public static List<NotificationPhoto> GetCompanyId(int companyid, int notificationID, NotificationType type)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          List<NotificationPhoto> existingPhoto = context.NotificationPhoto.Where(p => p.NotificationID == notificationID && p.Type == type && p.CompanyID == companyid).ToList();
          return existingPhoto;
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine($"NotificationPhoto bilgisi alınırken hata: {ex.Message}");
        List<NotificationPhoto> photo = new List<NotificationPhoto>();
        return photo;
      }
    }

    public static NotificationPhoto GetFirstNotificationIDType(int userid, int notificationID, NotificationType type)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var existingPhoto = context.NotificationPhoto.FirstOrDefault(p => p.ID == notificationID && p.Type == type && p.UserID == userid);
          return existingPhoto;
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine($"NotificationPhoto bilgisi alınırken hata: {ex.Message}");
        NotificationPhoto photo = new NotificationPhoto();
        return photo;
      }
    }


    public static void DeleteImages(string deletedPhotos, int userId)
    {
      if (!string.IsNullOrEmpty(deletedPhotos))
      {
        var deletedPhotoIds = JsonConvert.DeserializeObject<List<int>>(deletedPhotos);

        foreach (var photoId in deletedPhotoIds)
        {
          var photoToDelete = NotificationPhotoService.GetFirstNotificationIDType(userId, photoId, NotificationType.Infringement);
          if (photoToDelete != null)
          {
            var photoPath = Path.Combine("wwwroot", photoToDelete.PhotoUrl.TrimStart('/'));
            if (System.IO.File.Exists(photoPath))
            {
              System.IO.File.Delete(photoPath);
            }

            DeletePhoto(photoToDelete.ID);
          }
        }
      }
    }
    public static bool DeletePhoto(int id)
    {
      try
      {
        using (var context = new HcDbContext())
        {
          var result = context.NotificationPhoto.FirstOrDefault(p => p.ID == id);
          if (result != null)
          {
            context.NotificationPhoto.Remove(result);
            context.SaveChanges();
            return true;
          }
          else
          {
            return false;
          }

        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"NotificationPhoto silinirken hata: {ex.Message}");
        return false;
      }
    }


    public static string SaveAudioFile(IFormFile audioFile)
    {
      if (audioFile == null) return null;

      var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio", audioFile.FileName);

      using (var fileStream = new FileStream(audioFilePath, FileMode.Create))
      {
        audioFile.CopyTo(fileStream);
      }

      return audioFile.FileName;
    }



  }
}
