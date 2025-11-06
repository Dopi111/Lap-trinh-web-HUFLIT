using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveGuys.Models
{
    /// <summary>
    /// Model quản lý hình ảnh media (products, banners, posts, etc.)
    /// </summary>
    [Table("MediaImages")]
    public class MediaImage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hình ảnh")]
        [StringLength(200)]
        [Display(Name = "Tên hình ảnh")]
        public string ImageName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string ImagePath { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Loại hình ảnh")]
        public string ImageType { get; set; } // "Product", "Banner", "Post", "Gallery", etc.

        [Display(Name = "Kích thước (KB)")]
        public long? FileSize { get; set; }

        [StringLength(10)]
        [Display(Name = "Định dạng")]
        public string FileExtension { get; set; } // .jpg, .png, etc.

        [Display(Name = "Chiều rộng (px)")]
        public int? Width { get; set; }

        [Display(Name = "Chiều cao (px)")]
        public int? Height { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        // Computed property - Full URL
        [NotMapped]
        public string FullImageUrl
        {
            get { return string.IsNullOrEmpty(ImagePath) ? "/images/placeholder.png" : ImagePath; }
        }
    }
}
