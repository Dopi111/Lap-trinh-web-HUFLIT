using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FiveGuys.Models;

namespace FiveGuys.CRUD_ADMIN
{
    /// <summary>
    /// Controller quản lý Posts/Articles với hình ảnh
    /// </summary>
    public class PostsController : Controller
    {
        private FiveGuysProductEntities4 db = new FiveGuysProductEntities4();

        // GET: Posts
        public ActionResult Index(int? categoryId, bool? isFeatured)
        {
            var posts = db.Posts.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                posts = posts.Where(p => p.CategoryId == categoryId);
            }

            if (isFeatured.HasValue)
            {
                posts = posts.Where(p => p.IsFeatured == isFeatured.Value);
            }

            ViewBag.Categories = db.Categories.ToList();
            return View(posts.OrderByDescending(p => p.PublishedDate).ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            // Increment view count
            post.ViewCount++;
            db.SaveChanges();

            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            PopulateCategoriesDropdown();
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // Allow HTML content
        public ActionResult Create(Post post, HttpPostedFileBase featuredImage, HttpPostedFileBase thumbnailImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Auto-generate slug if not provided
                    if (string.IsNullOrEmpty(post.Slug))
                    {
                        post.Slug = GenerateSlug(post.Title);
                    }

                    // Check if slug already exists
                    if (db.Posts.Any(p => p.Slug == post.Slug))
                    {
                        ModelState.AddModelError("Slug", "Slug này đã tồn tại. Vui lòng chọn slug khác.");
                        PopulateCategoriesDropdown();
                        return View(post);
                    }

                    // Handle featured image upload
                    if (featuredImage != null && featuredImage.ContentLength > 0)
                    {
                        var result = SaveUploadedFile(featuredImage, "posts");
                        if (result.Success)
                        {
                            post.FeaturedImage = result.FilePath;
                        }
                        else
                        {
                            ModelState.AddModelError("featuredImage", result.ErrorMessage);
                        }
                    }

                    // Handle thumbnail image upload
                    if (thumbnailImage != null && thumbnailImage.ContentLength > 0)
                    {
                        var result = SaveUploadedFile(thumbnailImage, "posts");
                        if (result.Success)
                        {
                            post.ThumbnailImage = result.FilePath;
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        PopulateCategoriesDropdown();
                        return View(post);
                    }

                    // Set metadata
                    post.CreatedDate = DateTime.Now;
                    post.PublishedDate = DateTime.Now;
                    post.IsActive = true;
                    post.ViewCount = 0;

                    // Auto-generate meta tags if not provided
                    if (string.IsNullOrEmpty(post.MetaTitle))
                    {
                        post.MetaTitle = post.Title;
                    }
                    if (string.IsNullOrEmpty(post.MetaDescription))
                    {
                        post.MetaDescription = post.ShortDescription;
                    }

                    db.Posts.Add(post);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm bài viết thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu bài viết: " + ex.Message);
            }

            PopulateCategoriesDropdown();
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            PopulateCategoriesDropdown();
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // Allow HTML content
        public ActionResult Edit(Post post, HttpPostedFileBase featuredImage, HttpPostedFileBase thumbnailImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingPost = db.Posts.Find(post.Id);
                    if (existingPost == null)
                    {
                        return HttpNotFound();
                    }

                    // Check if slug already exists (excluding current post)
                    if (db.Posts.Any(p => p.Slug == post.Slug && p.Id != post.Id))
                    {
                        ModelState.AddModelError("Slug", "Slug này đã tồn tại. Vui lòng chọn slug khác.");
                        PopulateCategoriesDropdown();
                        return View(post);
                    }

                    // Handle featured image upload if new file is provided
                    if (featuredImage != null && featuredImage.ContentLength > 0)
                    {
                        DeletePhysicalFile(existingPost.FeaturedImage);
                        var result = SaveUploadedFile(featuredImage, "posts");
                        if (result.Success)
                        {
                            existingPost.FeaturedImage = result.FilePath;
                        }
                        else
                        {
                            ModelState.AddModelError("featuredImage", result.ErrorMessage);
                            PopulateCategoriesDropdown();
                            return View(post);
                        }
                    }

                    // Handle thumbnail image upload if new file is provided
                    if (thumbnailImage != null && thumbnailImage.ContentLength > 0)
                    {
                        DeletePhysicalFile(existingPost.ThumbnailImage);
                        var result = SaveUploadedFile(thumbnailImage, "posts");
                        if (result.Success)
                        {
                            existingPost.ThumbnailImage = result.FilePath;
                        }
                    }

                    // Update properties
                    existingPost.Title = post.Title;
                    existingPost.Slug = post.Slug;
                    existingPost.ShortDescription = post.ShortDescription;
                    existingPost.Content = post.Content;
                    existingPost.CategoryId = post.CategoryId;
                    existingPost.Tags = post.Tags;
                    existingPost.DisplayOrder = post.DisplayOrder;
                    existingPost.IsActive = post.IsActive;
                    existingPost.IsFeatured = post.IsFeatured;
                    existingPost.PublishedDate = post.PublishedDate;
                    existingPost.Author = post.Author;
                    existingPost.MetaTitle = post.MetaTitle;
                    existingPost.MetaDescription = post.MetaDescription;
                    existingPost.MetaKeywords = post.MetaKeywords;
                    existingPost.ModifiedDate = DateTime.Now;

                    db.Entry(existingPost).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật bài viết thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật bài viết: " + ex.Message);
            }

            PopulateCategoriesDropdown();
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Post post = db.Posts.Find(id);
                if (post != null)
                {
                    // Delete physical files
                    DeletePhysicalFile(post.FeaturedImage);
                    DeletePhysicalFile(post.ThumbnailImage);

                    db.Posts.Remove(post);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Xóa bài viết thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa bài viết: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        #region Helper Methods

        /// <summary>
        /// Generate URL-friendly slug from title
        /// </summary>
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title))
                return "";

            // Convert to lowercase
            string slug = title.ToLower();

            // Replace Vietnamese characters
            slug = slug.Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a");
            slug = slug.Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a");
            slug = slug.Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a");
            slug = slug.Replace("đ", "d");
            slug = slug.Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e");
            slug = slug.Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e");
            slug = slug.Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i");
            slug = slug.Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o");
            slug = slug.Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o");
            slug = slug.Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o");
            slug = slug.Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u");
            slug = slug.Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u");
            slug = slug.Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");

            // Remove special characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }

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

        private void PopulateCategoriesDropdown()
        {
            ViewBag.CategoryList = new SelectList(db.Categories, "Id", "TenLoai");
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
