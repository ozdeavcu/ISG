using CARBONISG.DataAccess;
using CARBONISG.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace CARBONISGMOBILE.Controllers
{
  [Authorize]
  public class NotificationController : BaseController
  { 
    private readonly InfringementNotificationService _infringementService;
    private readonly ComplaintAndWishService _complaintAndWishService;
    private readonly NearMissService _nearMissService;
    private readonly IncidentService _incidentService;


    public NotificationController(InfringementNotificationService infringementService, ComplaintAndWishService complaintAndWishService, NearMissService nearMissService, IncidentService incidentService)
    {
      _infringementService = infringementService;
      _complaintAndWishService = complaintAndWishService;
      _nearMissService = nearMissService;
      _incidentService = incidentService;

    }

    protected IActionResult CheckModelNotNull<T>(T model)
    {
      if (model == null)
      {
        SetErrorMessage("İlgili bildirim bulunamadı.");
        return RedirectToAction("Index", "Home");
      }

      return null;
    }
    private byte[] ConvertToBytes(IFormFile file)
    {
      using (var memoryStream = new MemoryStream())
      {
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
      }
    }

    #region Uygunsuzluk Bildirimi
    public IActionResult NotificationInfringementEdit(string? page,int id = 0)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        InfringementNotificationModel model = null;

        if (id <= 0)
        {
          model = new InfringementNotificationModel
          {
            ReportedAt = DateTime.Now.Date,
            ReportedAtTime = DateTime.Today.Add(DateTime.Now.TimeOfDay),
            CompanyID = CompanyID
          };
        }
        else
        {
          model = _infringementService.GetInfringementById(id);
          model.Photos = NotificationPhotoService.GetNotificationIDType(UserId, id, NotificationType.Infringement);
        }

        var result = CheckModelNotNull(model);
        if (result != null) return result;
        if (!string.IsNullOrEmpty(page))
        {
          TempData["page"] = page;
          TempData.Keep("page");
        }
  
        model.FullName = UserService.GetUserFullName(UserId) ?? "Bilgiler bulunamadı";

        return View(model);
      });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NotificationInfringementSave(InfringementNotificationModel model, List<IFormFile>? MultiImages, IFormFile? audioFile)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (!User.Identity.IsAuthenticated || model.UserID!=UserId)
        {
          SetErrorMessage("Yetkisiz işlem. Bu sayfaya erişim izniniz yok.");
          HttpContext.SignOutAsync();
          return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {         
          SetErrorMessage("Veri doğrulama hatası");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
        }

        if (model == null)
        {
          SetErrorMessage("Geçersiz model verisi");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
        }

        if (audioFile != null && audioFile.Length > 0)
        {
          try
          {
            byte[] audioData = ConvertToBytes(audioFile); 
            if (audioFile.ContentType != "audio/wav" && audioFile.ContentType != "audio/mpeg")
            {
              SetErrorMessage("Geçersiz dosya türü.");
              return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
            }
            model.AudioData = audioData;
            if (model.AudioData != null)
            {
          
              model.AudioUrl = Convert.ToBase64String(model.AudioData);
              var audioFileName = audioFile.FileName;

              //TODO:SES KAYITLARININ GÜVENLİ OLMASI İÇİN SUNUCUDA KAYIT EDİLMESİ GEREK.
              //var audioDirectory = Path.Combine(Directory.GetCurrentDirectory(), "private_audio_files");
              //Directory.CreateDirectory(audioDirectory);
              //var audioFilePath = Path.Combine(audioDirectory, audioFileName);
              //System.IO.File.WriteAllBytes(audioFilePath, audioData);

              var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", audioFileName);
              if (System.IO.File.Exists(audioFilePath))
              {
                System.IO.File.Delete(audioFilePath);
              }
              byte[] audioBytes = Convert.FromBase64String(model.AudioUrl);
              System.IO.File.WriteAllBytes(audioFilePath, audioBytes);
              model.AudioUrl = $"/uploads/audio/{audioFileName}";
            }
          }
          catch (Exception)
          {
            SetErrorMessage("Ses dosyası işlenirken bir hata oluştu!");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
          }
        }

        var result = _infringementService.Save(model);
        if (!result)
        {
          SetErrorMessage("Uygunsuzluk bildirimi kaydedilemedi!");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
        }

        if (MultiImages != null && MultiImages.Any())
        {
          try
          {
            NotificationPhotoService.SaveImages(MultiImages, model.ID, model.UserID, model.CompanyID);
          }
          catch (Exception ex)
          {
            SetErrorMessage("Fotoğraflar kaydedilirken bir hata oluştu:");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
          }
        }

        SetSuccessMessage("Uygunsuzluk bildirimi başarıyla kaydedildi!");

        return Json(new { success = true, redirectUrl = "/Notification/NotificationUserList?type=Infringement" });
      });
    }

    #endregion

    #region Ramak Kala
    public IActionResult NotificationNearMissEdit(string? page,int id=0)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        NearMissNotificationModel model;
        if (id <= 0)
        {
          model = new NearMissNotificationModel();
          model.ReportedAt = DateTime.Now.Date;
          model.ReportedAtTime = DateTime.Now;
          model.CompanyID = CompanyID;
        }
        else
        {
          model = _nearMissService.GetNearMissById(id);
          model.Photos = NotificationPhotoService.GetNotificationIDType(UserId, id, NotificationType.Infringement);
        }
        var result = CheckModelNotNull(model);
        if (result != null) return result;
        model.FullName = UserService.GetUserFullName(UserId) ?? "Bilgiler bulunamadı";
        if (!string.IsNullOrEmpty(page))
        {
          TempData["page"] = page;
          TempData.Keep("page");
        }
        return View(model);

      });
      
    }

    public IActionResult NotificationNearMissSave(NearMissNotificationModel model, List<IFormFile>? MultiImages, IFormFile? audioFile)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (!User.Identity.IsAuthenticated || model.UserID != UserId)
        {
          SetErrorMessage("Yetkisiz işlem. Bu sayfaya erişim izniniz yok.");
          HttpContext.SignOutAsync();
          return RedirectToAction("Login", "Home");
        }
        if (!ModelState.IsValid)
        {
          SetErrorMessage("Veri doğrulama hatası");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
        }
        if (model == null)
        {
          SetErrorMessage("Geçersiz model verisi");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
        }

        if (audioFile != null && audioFile.Length > 0)
        {
          try
          {
            byte[] audioData = ConvertToBytes(audioFile);
            if (audioFile.ContentType != "audio/wav" && audioFile.ContentType != "audio/mpeg")
            {
              SetErrorMessage("Geçersiz dosya türü.");
              return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
            }
            model.AudioData = audioData;
            if (model.AudioData != null)
            {
              model.AudioUrl = Convert.ToBase64String(model.AudioData);
              var audioFileName = audioFile.FileName;
              var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", audioFileName);
              if (System.IO.File.Exists(audioFilePath))
              {
                System.IO.File.Delete(audioFilePath);
              }
              byte[] audioBytes = Convert.FromBase64String(model.AudioUrl);
              System.IO.File.WriteAllBytes(audioFilePath, audioBytes);
              model.AudioUrl = $"/uploads/audio/{audioFileName}";
            }
          }
          catch (Exception ex)
          {
            SetErrorMessage("Ses dosyası işlenirken bir hata oluştu!");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
          }
        }
   
          var result = _nearMissService.Save(model);

          if (!result)
          {
          SetErrorMessage("Ramak Kala bildirimi kaydedilemedi!");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
          }

        if (MultiImages != null && MultiImages.Any())
        {
          try
          {
            NotificationPhotoService.SaveImages(MultiImages, model.ID, model.UserID, model.CompanyID);
          }
          catch (Exception ex)
          {
            SetErrorMessage("Fotoğraflar kaydedilirken bir hata oluştu:");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });
          }
        }

        SetSuccessMessage("Ramak Kala bildirimi başarıyla kaydedildi!");

        return Json(new { success = true, redirectUrl = "/Notification/NotificationUserList?type=NearMiss" });

      });
    }
    #endregion

    #region Dilek Şikayet

    public IActionResult ComplaintAndWishEdit(string? page, int id=0)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        ComplaintAndWishNotificationModel model;
        if (id <= 0)
        {
          model = new ComplaintAndWishNotificationModel();
          model.ReportedAt = DateTime.Now.Date;
          model.ReportedAtTime = DateTime.Now;
          model.CompanyID = CompanyID;
        }
        else
        {
          model = _complaintAndWishService.GetComplaintAndWishById(id);
          model.Photos = NotificationPhotoService.GetNotificationIDType(UserId, id, NotificationType.Infringement);

        }
        var result = CheckModelNotNull(model);
        if (result != null) return result;
        if (!string.IsNullOrEmpty(page))
        {
          TempData["page"] = page;
          TempData.Keep("page");
        }
        model.FullName = UserService.GetUserFullName(UserId);

        return View(model);
      });
    }

    public IActionResult ComplaintAndWishSave(ComplaintAndWishNotificationModel model, List<IFormFile> MultiImages, IFormFile? audioFile)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (!User.Identity.IsAuthenticated || model.UserID != UserId)
        {
          SetErrorMessage("Yetkisiz işlem. Bu sayfaya erişim izniniz yok.");
          HttpContext.SignOutAsync();
          return RedirectToAction("Login", "Home");
        }
        if (!ModelState.IsValid)
        {
          SetErrorMessage("Veri doğrulama hatası");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
        }
        if (model == null)
        {
          SetErrorMessage("Geçersiz model verisi");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
        }

        if (audioFile != null && audioFile.Length > 0)
        {
          try
          {
            byte[] audioData = ConvertToBytes(audioFile);
            if (audioFile.ContentType != "audio/wav" && audioFile.ContentType != "audio/mpeg")
            {
              SetErrorMessage("Geçersiz dosya türü.");
              return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
            }
            model.AudioData = audioData;
            if (model.AudioData != null)
            {
              model.AudioUrl = Convert.ToBase64String(model.AudioData);
              var audioFileName = audioFile.FileName;
              var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", audioFileName);
              if (System.IO.File.Exists(audioFilePath))
              {
                System.IO.File.Delete(audioFilePath);
              }
              byte[] audioBytes = Convert.FromBase64String(model.AudioUrl);
              System.IO.File.WriteAllBytes(audioFilePath, audioBytes);
              model.AudioUrl = $"/uploads/audio/{audioFileName}";
            }

          }
          catch (Exception ex)
          {
            SetErrorMessage("Ses dosyası işlenirken bir hata oluştu!");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
          }
        }

        var result = _complaintAndWishService.Save(model);

        if (!result)
        {
          SetErrorMessage("Dilek Ve Şikayet bildirimi kaydedilemedi!");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
        }

        if (MultiImages != null && MultiImages.Any())
        {
          try
          {
            NotificationPhotoService.SaveImages(MultiImages, model.ID, model.UserID, model.CompanyID);
          }
          catch (Exception ex)
          {
            SetErrorMessage("Fotoğraflar kaydedilirken bir hata oluştu:");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });
          }
        }

        SetSuccessMessage("Dilek Ve Şikayet bildirimi başarıyla kaydedildi!");

        return Json(new { success = true, redirectUrl = "/Notification/NotificationUserList?type=ComplaintAndWish" });

      });
    }
    #endregion

    #region Olay Bildirimi
    public IActionResult IncidentEdit(string? page, int id=0)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        IncidentNotificationModel model;
        if (id <= 0)
        {
          model = new IncidentNotificationModel();
          model.ReportedAt = DateTime.Now.Date;
          model.ReportedAtTime = DateTime.Now;
          model.CompanyID = CompanyID;
        }
        else
        {
          model = _incidentService.GetIncidentById(id);
          model.Photos = NotificationPhotoService.GetNotificationIDType(UserId, id, NotificationType.Infringement);
        }
        var result = CheckModelNotNull(model);
        if (result != null) return result;
        if (!string.IsNullOrEmpty(page))
        {
          TempData["page"] = page;
          TempData.Keep("page");
        }
        model.FullName = UserService.GetUserFullName(UserId);

        return View(model);
      });
    }

    public IActionResult IncidentSave(IncidentNotificationModel model, List<IFormFile> MultiImages, IFormFile? audioFile)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        if (!User.Identity.IsAuthenticated || model.UserID != UserId)
        {
          SetErrorMessage("Yetkisiz işlem. Bu sayfaya erişim izniniz yok.");
          HttpContext.SignOutAsync();
          return RedirectToAction("Login", "Home");
        }
        if (!ModelState.IsValid)
        {
          SetErrorMessage("Veri doğrulama hatası");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
        }
        if (model == null)
        {
          SetErrorMessage("Geçersiz model verisi");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
        }

        if (audioFile != null && audioFile.Length > 0)
        {
          try
          {
            byte[] audioData = ConvertToBytes(audioFile);
            if (audioFile.ContentType != "audio/wav" && audioFile.ContentType != "audio/mpeg")
            {
              SetErrorMessage("Geçersiz dosya türü.");
              return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
            }
            model.AudioData = audioData;
            if (model.AudioData != null)
            {
              model.AudioUrl = Convert.ToBase64String(model.AudioData);
              var audioFileName = audioFile.FileName;
              var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", audioFileName);
              if (System.IO.File.Exists(audioFilePath))
              {
                System.IO.File.Delete(audioFilePath);
              }
              byte[] audioBytes = Convert.FromBase64String(model.AudioUrl);
              System.IO.File.WriteAllBytes(audioFilePath, audioBytes);
              model.AudioUrl = $"/uploads/audio/{audioFileName}";
            }
          }
          catch (Exception)
          {
            SetErrorMessage("Ses dosyası işlenirken bir hata oluştu!");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
          }
        }

        var result = _incidentService.Save(model);

        if (!result)
        {
          SetErrorMessage("Olay bildirimi kaydedilemedi!");
          return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
        }

        if (MultiImages != null && MultiImages.Any())
        {
          try
          {
            NotificationPhotoService.SaveImages(MultiImages, model.ID, model.UserID, model.CompanyID);
          }
          catch (Exception ex)
          {
            SetErrorMessage("Fotoğraflar kaydedilirken bir hata oluştu:");
            return Json(new { success = false, redirectUrl = "/Notification/NotificationUserList?type=Incident" });
          }
        }

        SetSuccessMessage("Olay bildirimi başarıyla kaydedildi!");

        return Json(new { success = true, redirectUrl = "/Notification/NotificationUserList?type=Incident" });

      });
    }
    #endregion

    #region Kullanıcı Bildirim Listesi
    public IActionResult NotificationUserList(NotificationType type ,int sortOption=0)
    {
      return ExecuteWithExceptionHandling(() =>
      {
        List<NotificationModel> notifications = type switch
        {
          NotificationType.Incident => _incidentService.IncidentListByUserId(UserId)
              .Select(x => new NotificationModel
              {
                Id = x.ID,
                UserID = x.UserID,
                CompanyID = x.CompanyID,
                Title = x.IncidentSection,
                Description = x.IncidentDescription,
                Date = x.ReportedAtTime,
                Type = NotificationType.Incident
              }).ToList(),
          NotificationType.NearMiss => _nearMissService.NearMissListByUserId(UserId)
              .Select(x => new NotificationModel
              {
                Id = x.ID,
                UserID = x.UserID,
                CompanyID = x.CompanyID,
                Title = x.NearMissLocation,
                Description = x.NearMissDescription,
                Date = x.ReportedAtTime,
                Type = NotificationType.NearMiss
              }).ToList(),
          NotificationType.ComplaintAndWish => _complaintAndWishService.ComplaintAndWishListByUserId(UserId)
              .Select(x => new NotificationModel
              {
                Id = x.ID,
                UserID = x.UserID,
                CompanyID = x.CompanyID,
                ComplaintOrWish = x.ComplaintOrWish,
                Description = x.ComplaintOrWishDescription,
                Date = x.ReportedAtTime,
                Type = NotificationType.ComplaintAndWish
              }).ToList(),
          NotificationType.Infringement => _infringementService.InfringementListByUserId(UserId)
              .Select(x => new NotificationModel
              {
                Id = x.ID,
                UserID = x.UserID,
                CompanyID = x.CompanyID,
                Title = x.InfringementSection,
                Description = x.InfringementDescription,
                Date = x.ReportedAtTime,
                Type = NotificationType.Infringement
              }).ToList(),
          _ => new List<NotificationModel>()
        };

        notifications = notifications.Where(n => n.UserID == UserId && n.CompanyID == CompanyID).ToList();

        var sortOptionQuery = Request.Query["defaultSelectSm"];
        if (int.TryParse(sortOptionQuery, out int parsedSortOption))
        {
          sortOption = parsedSortOption;
        }

        if (sortOption > 0)
        {
          notifications = SortNotifications(notifications, sortOption);
          ViewData["SortOption"] = sortOption;
        }

        return View(notifications);
      });
    }

    public IActionResult NotificationUserEdit(NotificationType type,int id,string? page)
    {
      try
      {
        if (type == NotificationType.Infringement)
        {
          return RedirectToAction("NotificationInfringementEdit", new { id = id ,page=page});
        }
        else if (type == NotificationType.NearMiss)
        {
          return RedirectToAction("NotificationNearMissEdit", new { id = id,page = page });
        }
        else if (type == NotificationType.Incident)
        {
          return RedirectToAction("IncidentEdit", new { id = id , page = page });
        }
        else if (type == NotificationType.ComplaintAndWish)
        {
          return RedirectToAction("ComplaintAndWishEdit", new { id = id , page = page });
        }
        else
        {
          TempData["ErrorMessage"] = "Bir şeyler ters gitti. Lütfen tekrar deneyin.";
          return RedirectToAction("Index", "Home");
        }
  
      }
      catch (Exception ex)
      {
        TempData["ErrorMessage"] = "Bir şeyler ters gitti. Lütfen tekrar deneyin.";
        return View();
      }
    }

    private List<NotificationModel> SortNotifications(List<NotificationModel> notifications, int sortOption)
    {
      switch (sortOption)
      {
        case 1: // Son Eklenen
          notifications = notifications.OrderByDescending(x => x.Date).ToList();
          break;
        case 2: // Eskiden Yeniye
          notifications = notifications.OrderBy(x => x.Date).ToList();
          break;
        case 3: // Yeniden Eskiye
          notifications = notifications.OrderByDescending(x => x.Date).ToList();
          break;
        case 4: // Başlığa Göre (A-Z)
          notifications = notifications.OrderBy(x => x.Title).ToList();
          break;
        default:
          break;
      }
      return notifications;
    }
    #endregion
  }
}
