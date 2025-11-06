using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FiveGuys.Models;

namespace FiveGuys.CRUD_ADMIN
{
    /// <summary>
    /// Controller quản lý Hero Banners
    /// </summary>
    public class HeroBannersController : Controller
    {
        private FiveGuysProductEntities4 db = new FiveGuysProductEntities4();

        // GET: HeroBanners
        public ActionResult Index(string position = "")
        {
            var banners = db.HeroBanners.AsQueryable();

            if (!string.IsNullOrEmpty(position))
            {
                banners = banners.Where(b => b.BannerPosition == position);
            }

            ViewBag.BannerPositions = db.HeroBanners
                .Select(b => b.BannerPosition)
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            return View(banners.OrderBy(b => b.DisplayOrder).ToList());
        }

        // GET: HeroBanners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HeroBanner heroBanner = db.HeroBanners.Find(id);
            if (heroBanner == null)
            {
                return HttpNotFound();
            }

            return View(heroBanner);
        }

        // GET: HeroBanners/Create
        public ActionResult Create()
        {
            PopulateBannerPositionsDropdown();
            return View();
        }

        // POST: HeroBanners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HeroBanner heroBanner, HttpPostedFileBase desktopImage, HttpPostedFileBase mobileImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle desktop image upload
                    if (desktopImage != null && desktopImage.ContentLength > 0)
                    {
                        var result = SaveUploadedFile(desktopImage, "banners");
                        if (result.Success)
                        {
                            heroBanner.ImagePath = result.FilePath;
                        }
                        else
                        {
                            ModelState.AddModelError("desktopImage", result.ErrorMessage);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("desktopImage", "Vui lòng chọn hình ảnh banner");
                    }

                    // Handle mobile image upload (optional)
                    if (mobileImage != null && mobileImage.ContentLength > 0)
                    {
                        var result = SaveUploadedFile(mobileImage, "banners");
                        if (result.Success)
                        {
                            heroBanner.MobileImagePath = result.FilePath;
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        PopulateBannerPositionsDropdown();
                        return View(heroBanner);
                    }

                    // Set metadata
                    heroBanner.CreatedDate = DateTime.Now;
                    heroBanner.IsActive = true;

                    db.HeroBanners.Add(heroBanner);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm banner thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu banner: " + ex.Message);
            }

            PopulateBannerPositionsDropdown();
            return View(heroBanner);
        }

        // GET: HeroBanners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HeroBanner heroBanner = db.HeroBanners.Find(id);
            if (heroBanner == null)
            {
                return HttpNotFound();
            }

            PopulateBannerPositionsDropdown();
            return View(heroBanner);
        }

        // POST: HeroBanners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HeroBanner heroBanner, HttpPostedFileBase desktopImage, HttpPostedFileBase mobileImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingBanner = db.HeroBanners.Find(heroBanner.Id);
                    if (existingBanner == null)
                    {
                        return HttpNotFound();
                    }

                    // Handle desktop image upload if new file is provided
                    if (desktopImage != null && desktopImage.ContentLength > 0)
                    {
                        DeletePhysicalFile(existingBanner.ImagePath);
                        var result = SaveUploadedFile(desktopImage, "banners");
                        if (result.Success)
                        {
                            existingBanner.ImagePath = result.FilePath;
                        }
                        else
                        {
                            ModelState.AddModelError("desktopImage", result.ErrorMessage);
                            PopulateBannerPositionsDropdown();
                            return View(heroBanner);
                        }
                    }

                    // Handle mobile image upload if new file is provided
                    if (mobileImage != null && mobileImage.ContentLength > 0)
                    {
                        DeletePhysicalFile(existingBanner.MobileImagePath);
                        var result = SaveUploadedFile(mobileImage, "banners");
                        if (result.Success)
                        {
                            existingBanner.MobileImagePath = result.FilePath;
                        }
                    }

                    // Update properties
                    existingBanner.Title = heroBanner.Title;
                    existingBanner.Subtitle = heroBanner.Subtitle;
                    existingBanner.Description = heroBanner.Description;
                    existingBanner.ButtonText = heroBanner.ButtonText;
                    existingBanner.ButtonLink = heroBanner.ButtonLink;
                    existingBanner.DisplayOrder = heroBanner.DisplayOrder;
                    existingBanner.BannerPosition = heroBanner.BannerPosition;
                    existingBanner.IsActive = heroBanner.IsActive;
                    existingBanner.StartDate = heroBanner.StartDate;
                    existingBanner.EndDate = heroBanner.EndDate;
                    existingBanner.BackgroundColor = heroBanner.BackgroundColor;
                    existingBanner.TextColor = heroBanner.TextColor;
                    existingBanner.ModifiedDate = DateTime.Now;

                    db.Entry(existingBanner).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật banner thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật banner: " + ex.Message);
            }

            PopulateBannerPositionsDropdown();
            return View(heroBanner);
        }

        // GET: HeroBanners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HeroBanner heroBanner = db.HeroBanners.Find(id);
            if (heroBanner == null)
            {
                return HttpNotFound();
            }

            return View(heroBanner);
        }

        // POST: HeroBanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                HeroBanner heroBanner = db.HeroBanners.Find(id);
                if (heroBanner != null)
                {
                    // Delete physical files
                    DeletePhysicalFile(heroBanner.ImagePath);
                    DeletePhysicalFile(heroBanner.MobileImagePath);

                    db.HeroBanners.Remove(heroBanner);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Xóa banner thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa banner: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        #region Helper Methods

        private UploadResult SaveUploadedFile(HttpPostedFileBase file, string subFolder = "uploads")
        {
            var result = new UploadResult();

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    result.ErrorMessage = "Chỉ chấp nhận file ảnh: JPG, JPEG, PNG, GIF, WEBP, SVG";
                    return result;
                }

                if (file.ContentLength > 5 * 1024 * 1024)
                {
                    result.ErrorMessage = "Kích thước file không được vượt quá 5MB";
                    return result;
                }

                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

                var uploadFolder = Server.MapPath($"~/images/{subFolder}/");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                file.SaveAs(filePath);

                result.Success = true;
                result.FilePath = $"/images/{subFolder}/" + uniqueFileName;
                result.FileExtension = fileExtension;
                result.FileSize = file.ContentLength;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Lỗi khi upload file: " + ex.Message;
            }

            return result;
        }

        private void DeletePhysicalFile(string relativePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(relativePath))
                {
                    var physicalPath = Server.MapPath("~" + relativePath);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch { }
        }

        private void PopulateBannerPositionsDropdown()
        {
            var positions = new[]
            {
                new { Value = "Home", Text = "Trang chủ" },
                new { Value = "Shop", Text = "Cửa hàng" },
                new { Value = "About", Text = "Giới thiệu" },
                new { Value = "Contact", Text = "Liên hệ" },
                new { Value = "Why", Text = "Tại sao chọn chúng tôi" },
                new { Value = "Testimonial", Text = "Đánh giá" }
            };

            ViewBag.BannerPositionList = new SelectList(positions, "Value", "Text");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private class UploadResult
        {
            public bool Success { get; set; }
            public string FilePath { get; set; }
            public string FileExtension { get; set; }
            public long FileSize { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
