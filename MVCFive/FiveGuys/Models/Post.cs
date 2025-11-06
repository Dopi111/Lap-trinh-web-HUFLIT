using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveGuys.Models
{
    /// <summary>
    /// Model quản lý bài viết/tin tức với hình ảnh
    /// </summary>
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề bài viết")]
        [StringLength(300)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập slug")]
        [StringLength(300)]
        [Display(Name = "Slug (URL)")]
        public string Slug { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [StringLength(500)]
        [Display(Name = "Hình ảnh đại diện")]
        public string FeaturedImage { get; set; }

        [StringLength(200)]
        [Display(Name = "Thumbnail")]
        public string ThumbnailImage { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }

        [StringLength(200)]
        [Display(Name = "Tags")]
        public string Tags { get; set; } // Comma separated: "Khuyến mãi, Tin tức, Sản phẩm mới"

        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; }

        [Display(Name = "Nổi bật")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Ngày xuất bản")]
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Tác giả")]
        public string Author { get; set; }

        [StringLength(100)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        // SEO Properties
        [StringLength(200)]
        [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }

        [StringLength(200)]
        [Display(Name = "Meta Keywords")]
        public string MetaKeywords { get; set; }

        // Navigation property
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // Computed properties
        [NotMapped]
        public string DisplayShortContent
        {
            get
            {
                if (string.IsNullOrEmpty(ShortDescription))
                {
                    return Content?.Length > 200 ? Content.Substring(0, 200) + "..." : Content;
                }
                return ShortDescription;
            }
        }

        [NotMapped]
        public string[] TagsList
        {
            get
            {
                return string.IsNullOrEmpty(Tags)
                    ? new string[0]
                    : Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
