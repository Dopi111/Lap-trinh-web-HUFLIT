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
    /// Controller quản lý Media Images với file upload
    /// </summary>
    public class MediaImagesController : Controller
    {
        private FiveGuysProductEntities4 db = new FiveGuysProductEntities4();

        // GET: MediaImages
        public ActionResult Index(string imageType = "")
        {
            var images = db.MediaImages.AsQueryable();

            if (!string.IsNullOrEmpty(imageType))
            {
                images = images.Where(i => i.ImageType == imageType);
            }

            ViewBag.ImageTypes = db.MediaImages
                .Select(i => i.ImageType)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            return View(images.OrderByDescending(i => i.CreatedDate).ToList());
        }

        // GET: MediaImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MediaImage mediaImage = db.MediaImages.Find(id);
            if (mediaImage == null)
            {
                return HttpNotFound();
            }

            return View(mediaImage);
        }

        // GET: MediaImages/Create
        public ActionResult Create()
        {
            PopulateImageTypesDropdown();
            return View();
        }

        // POST: MediaImages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MediaImage mediaImage, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle file upload
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var result = SaveUploadedFile(imageFile);
                        if (result.Success)
                        {
                            mediaImage.ImagePath = result.FilePath;
                            mediaImage.FileExtension = result.FileExtension;
                            mediaImage.FileSize = result.FileSize;
                        }
                        else
                        {
                            ModelState.AddModelError("", result.ErrorMessage);
                            PopulateImageTypesDropdown();
                            return View(mediaImage);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("imageFile", "Vui lòng chọn file hình ảnh");
                        PopulateImageTypesDropdown();
                        return View(mediaImage);
                    }

                    // Set metadata
                    mediaImage.CreatedDate = DateTime.Now;
                    mediaImage.IsActive = true;

                    db.MediaImages.Add(mediaImage);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm hình ảnh thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu hình ảnh: " + ex.Message);
            }

            PopulateImageTypesDropdown();
            return View(mediaImage);
        }

        // GET: MediaImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MediaImage mediaImage = db.MediaImages.Find(id);
            if (mediaImage == null)
            {
                return HttpNotFound();
            }

            PopulateImageTypesDropdown();
            return View(mediaImage);
        }

        // POST: MediaImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MediaImage mediaImage, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingImage = db.MediaImages.Find(mediaImage.Id);
                    if (existingImage == null)
                    {
                        return HttpNotFound();
                    }

                    // Handle file upload if new file is provided
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        // Delete old file
                        DeletePhysicalFile(existingImage.ImagePath);

                        // Save new file
                        var result = SaveUploadedFile(imageFile);
                        if (result.Success)
                        {
                            existingImage.ImagePath = result.FilePath;
                            existingImage.FileExtension = result.FileExtension;
                            existingImage.FileSize = result.FileSize;
                        }
                        else
                        {
                            ModelState.AddModelError("", result.ErrorMessage);
                            PopulateImageTypesDropdown();
                            return View(mediaImage);
                        }
                    }

                    // Update properties
                    existingImage.ImageName = mediaImage.ImageName;
                    existingImage.Description = mediaImage.Description;
                    existingImage.ImageType = mediaImage.ImageType;
                    existingImage.Width = mediaImage.Width;
                    existingImage.Height = mediaImage.Height;
                    existingImage.DisplayOrder = mediaImage.DisplayOrder;
                    existingImage.IsActive = mediaImage.IsActive;
                    existingImage.ModifiedDate = DateTime.Now;

                    db.Entry(existingImage).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật hình ảnh thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật hình ảnh: " + ex.Message);
            }

            PopulateImageTypesDropdown();
            return View(mediaImage);
        }

        // GET: MediaImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MediaImage mediaImage = db.MediaImages.Find(id);
            if (mediaImage == null)
            {
                return HttpNotFound();
            }

            return View(mediaImage);
        }

        // POST: MediaImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                MediaImage mediaImage = db.MediaImages.Find(id);
                if (mediaImage != null)
                {
                    // Delete physical file
                    DeletePhysicalFile(mediaImage.ImagePath);

                    db.MediaImages.Remove(mediaImage);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Xóa hình ảnh thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa hình ảnh: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        #region Helper Methods

        /// <summary>
        /// Lưu file upload và trả về thông tin file
        /// </summary>
        private UploadResult SaveUploadedFile(HttpPostedFileBase file)
        {
            var result = new UploadResult();

            try
            {
                // Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    result.ErrorMessage = "Chỉ chấp nhận file ảnh: JPG, JPEG, PNG, GIF, WEBP, SVG";
                    return result;
                }

                // Max file size: 5MB
                if (file.ContentLength > 5 * 1024 * 1024)
                {
                    result.ErrorMessage = "Kích thước file không được vượt quá 5MB";
                    return result;
                }

                // Generate unique filename
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

                // Save to /images/uploads/ folder
                var uploadFolder = Server.MapPath("~/images/uploads/");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                file.SaveAs(filePath);

                // Return relative path for database
                result.Success = true;
                result.FilePath = "/images/uploads/" + uniqueFileName;
                result.FileExtension = fileExtension;
                result.FileSize = file.ContentLength;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Lỗi khi upload file: " + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Xóa file vật lý trên server
        /// </summary>
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
            catch
            {
                // Ignore errors when deleting files
            }
        }

        /// <summary>
        /// Populate dropdown list cho Image Types
        /// </summary>
        private void PopulateImageTypesDropdown()
        {
            var imageTypes = new[]
            {
                new { Value = "Product", Text = "Hình ảnh sản phẩm" },
                new { Value = "Banner", Text = "Banner" },
                new { Value = "Post", Text = "Bài viết" },
                new { Value = "Gallery", Text = "Thư viện ảnh" },
                new { Value = "Icon", Text = "Icon" },
                new { Value = "Other", Text = "Khác" }
            };

            ViewBag.ImageTypeList = new SelectList(imageTypes, "Value", "Text");
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

        #region Helper Classes

        private class UploadResult
        {
            public bool Success { get; set; }
            public string FilePath { get; set; }
            public string FileExtension { get; set; }
            public long FileSize { get; set; }
            public string ErrorMessage { get; set; }
        }

        #endregion
    }
}
