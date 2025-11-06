using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveGuys.Models
{
    /// <summary>
    /// Model quản lý Hero Banner cho trang chủ và các trang khác
    /// </summary>
    [Table("HeroBanners")]
    public class HeroBanner
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề banner")]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Tiêu đề phụ")]
        public string Subtitle { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string ImagePath { get; set; }

        [StringLength(500)]
        [Display(Name = "Hình ảnh mobile")]
        public string MobileImagePath { get; set; }

        [StringLength(200)]
        [Display(Name = "Text Button")]
        public string ButtonText { get; set; }

        [StringLength(500)]
        [Display(Name = "Link Button")]
        public string ButtonLink { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Vị trí banner")]
        public string BannerPosition { get; set; } // "Home", "Shop", "About", "Contact", etc.

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Màu nền")]
        [StringLength(20)]
        public string BackgroundColor { get; set; }

        [Display(Name = "Màu chữ")]
        [StringLength(20)]
        public string TextColor { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        // Computed property
        [NotMapped]
        public bool IsCurrentlyActive
        {
            get
            {
                if (!IsActive) return false;
                var now = DateTime.Now;
                if (StartDate.HasValue && now < StartDate.Value) return false;
                if (EndDate.HasValue && now > EndDate.Value) return false;
                return true;
            }
        }
    }
}
