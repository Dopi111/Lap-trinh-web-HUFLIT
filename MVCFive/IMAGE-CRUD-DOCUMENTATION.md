# FiveGuys - Hệ Thống Quản Lý Hình Ảnh CRUD

## Tổng Quan

Backend hệ thống quản lý hình ảnh cho dự án FiveGuys, bao gồm:
- ✅ Quản lý Media Images (hình ảnh chung)
- ✅ Quản lý Hero Banners (banner trang chủ)
- ✅ Quản lý Posts (bài viết với hình ảnh)
- ✅ File upload với validation
- ✅ CRUD operations đầy đủ

## Công Nghệ Sử Dụng

- **Backend Framework**: ASP.NET MVC 5.2.9
- **ORM**: Entity Framework 6.4.4
- **Database**: SQL Server
- **Frontend**: Bootstrap 3.3.7, jQuery 3.4.1
- **Language**: C# (.NET Framework 4.8.1)

## Cấu Trúc Dự Án

```
MVCFive/FiveGuys/
├── Models/
│   ├── MediaImage.cs          # Model hình ảnh chung
│   ├── HeroBanner.cs          # Model banner
│   ├── Post.cs                # Model bài viết
│   └── Model1.Context.cs      # DbContext (đã cập nhật)
│
├── CRUD ADMIN/
│   ├── MediaImagesController.cs    # Controller quản lý hình ảnh
│   ├── HeroBannersController.cs    # Controller quản lý banner
│   └── PostsController.cs          # Controller quản lý bài viết
│
├── Views/
│   ├── MediaImages/
│   │   ├── Index.cshtml       # Danh sách hình ảnh
│   │   ├── Create.cshtml      # Thêm hình ảnh
│   │   ├── Edit.cshtml        # Sửa hình ảnh
│   │   └── Delete.cshtml      # Xóa hình ảnh
│   │
│   ├── HeroBanners/
│   │   ├── Index.cshtml       # Danh sách banner
│   │   └── Create.cshtml      # Thêm banner
│   │
│   └── Posts/
│       └── Index.cshtml       # Danh sách bài viết
│
└── images/
    ├── uploads/               # Thư mục lưu hình ảnh upload
    ├── banners/               # Thư mục lưu banner
    └── posts/                 # Thư mục lưu hình bài viết
```

## Hướng Dẫn Cài Đặt

### Bước 1: Cấu Hình Database

1. Mở **SQL Server Management Studio**
2. Chạy script `Database-ImageCRUD-Schema.sql`:
   ```sql
   -- Đường dẫn: /MVCFive/Database-ImageCRUD-Schema.sql
   ```

Script sẽ tạo:
- ✅ Bảng `MediaImages` - Lưu trữ hình ảnh
- ✅ Bảng `HeroBanners` - Lưu trữ banner
- ✅ Bảng `Posts` - Lưu trữ bài viết
- ✅ Indexes để tối ưu hiệu suất
- ✅ Sample data mẫu
- ✅ Views và Stored Procedures

### Bước 2: Cấu Hình Connection String

Kiểm tra file `Web.config` và đảm bảo connection string đúng:

```xml
<connectionStrings>
  <add name="FiveGuysProductEntities4"
       connectionString="metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;
       provider=System.Data.SqlClient;
       provider connection string=&quot;data source=YOUR_SERVER_NAME;
       initial catalog=FiveGuysProduct;
       integrated security=True;
       MultipleActiveResultSets=True;
       App=EntityFramework&quot;"
       providerName="System.Data.EntityClient" />
</connectionStrings>
```

**Thay đổi**: `data source=YOUR_SERVER_NAME` thành tên SQL Server của bạn.

### Bước 3: Build Project

1. Mở project trong **Visual Studio**
2. Restore NuGet packages:
   - Chuột phải vào Solution → **Restore NuGet Packages**
3. Build project:
   - **Build** → **Build Solution** (Ctrl+Shift+B)

### Bước 4: Chạy Ứng Dụng

1. Nhấn **F5** hoặc **Ctrl+F5** để chạy
2. Truy cập các URL sau để test:

## URL Routes - Các Trang Quản Lý

### Quản Lý Hình Ảnh (Media Images)
```
http://localhost:[PORT]/MediaImages
http://localhost:[PORT]/MediaImages/Create
http://localhost:[PORT]/MediaImages/Edit/[ID]
http://localhost:[PORT]/MediaImages/Delete/[ID]
```

### Quản Lý Hero Banners
```
http://localhost:[PORT]/HeroBanners
http://localhost:[PORT]/HeroBanners/Create
http://localhost:[PORT]/HeroBanners/Edit/[ID]
http://localhost:[PORT]/HeroBanners/Delete/[ID]
```

### Quản Lý Bài Viết (Posts)
```
http://localhost:[PORT]/Posts
http://localhost:[PORT]/Posts/Create
http://localhost:[PORT]/Posts/Edit/[ID]
http://localhost:[PORT]/Posts/Delete/[ID]
```

## Tính Năng Chính

### 1. Quản Lý Media Images

**Chức năng**:
- ✅ Upload hình ảnh (JPG, PNG, GIF, WEBP, SVG)
- ✅ Phân loại hình ảnh (Product, Banner, Post, Gallery, Icon, Other)
- ✅ Validation file size (max 5MB)
- ✅ Tự động lưu metadata (kích thước, extension, created date)
- ✅ Preview hình ảnh trước khi upload
- ✅ Filter theo loại hình ảnh
- ✅ Active/Inactive status

**Model Properties**:
```csharp
- Id
- ImageName          // Tên hình ảnh
- ImagePath          // Đường dẫn file
- Description        // Mô tả
- ImageType          // Loại: Product, Banner, Post, etc.
- FileSize           // Kích thước (bytes)
- FileExtension      // .jpg, .png, etc.
- Width, Height      // Kích thước ảnh (px)
- DisplayOrder       // Thứ tự hiển thị
- IsActive           // Trạng thái
- CreatedDate        // Ngày tạo
- CreatedBy          // Người tạo
```

### 2. Quản Lý Hero Banners

**Chức năng**:
- ✅ Upload banner cho desktop và mobile
- ✅ Cấu hình vị trí banner (Home, Shop, About, Contact, etc.)
- ✅ Thiết lập thời gian hiển thị (StartDate, EndDate)
- ✅ Tùy chỉnh button text và link
- ✅ Tùy chỉnh màu nền và màu chữ
- ✅ Thứ tự hiển thị
- ✅ Tự động kiểm tra banner đang active theo thời gian

**Model Properties**:
```csharp
- Id
- Title              // Tiêu đề
- Subtitle           // Tiêu đề phụ
- Description        // Mô tả
- ImagePath          // Hình desktop
- MobileImagePath    // Hình mobile (optional)
- ButtonText         // Text button
- ButtonLink         // Link button
- BannerPosition     // Home, Shop, About, etc.
- DisplayOrder       // Thứ tự
- IsActive           // Trạng thái
- StartDate          // Ngày bắt đầu
- EndDate            // Ngày kết thúc
- BackgroundColor    // Màu nền
- TextColor          // Màu chữ
```

### 3. Quản Lý Posts (Bài Viết)

**Chức năng**:
- ✅ Upload featured image và thumbnail
- ✅ Rich text editor cho nội dung
- ✅ Auto-generate slug từ tiêu đề (Vietnamese support)
- ✅ Phân loại theo danh mục
- ✅ Tagging system
- ✅ Featured posts
- ✅ View counter
- ✅ SEO meta tags (title, description, keywords)
- ✅ Published date scheduling

**Model Properties**:
```csharp
- Id
- Title              // Tiêu đề
- Slug               // URL-friendly slug
- ShortDescription   // Mô tả ngắn
- Content            // Nội dung HTML
- FeaturedImage      // Hình đại diện
- ThumbnailImage     // Thumbnail
- CategoryId         // Danh mục
- Tags               // Tags (comma-separated)
- ViewCount          // Lượt xem
- IsFeatured         // Bài viết nổi bật
- IsActive           // Trạng thái
- PublishedDate      // Ngày xuất bản
- Author             // Tác giả
- MetaTitle          // SEO Title
- MetaDescription    // SEO Description
- MetaKeywords       // SEO Keywords
```

## File Upload Specifications

### Định Dạng Được Chấp Nhận
```
- JPG/JPEG
- PNG
- GIF
- WEBP
- SVG
```

### Kích Thước Tối Đa
```
5 MB per file
```

### Thư Mục Lưu Trữ
```
/images/uploads/     - Media images
/images/banners/     - Hero banners
/images/posts/       - Post images
```

### Naming Convention
```
{original-name}_{timestamp}.{extension}

Ví dụ: product-image_20250106143025.jpg
```

## API Usage (Code Examples)

### 1. Lấy Danh Sách Hình Ảnh Theo Loại

```csharp
using (var db = new FiveGuysProductEntities4())
{
    var productImages = db.MediaImages
        .Where(i => i.ImageType == "Product" && i.IsActive)
        .OrderBy(i => i.DisplayOrder)
        .ToList();
}
```

### 2. Lấy Active Banners Theo Vị Trí

```csharp
using (var db = new FiveGuysProductEntities4())
{
    var now = DateTime.Now;
    var homeBanners = db.HeroBanners
        .Where(b => b.BannerPosition == "Home"
                 && b.IsActive
                 && (!b.StartDate.HasValue || b.StartDate <= now)
                 && (!b.EndDate.HasValue || b.EndDate >= now))
        .OrderBy(b => b.DisplayOrder)
        .ToList();
}
```

### 3. Lấy Featured Posts

```csharp
using (var db = new FiveGuysProductEntities4())
{
    var featuredPosts = db.Posts
        .Include(p => p.Category)
        .Where(p => p.IsActive && p.IsFeatured)
        .OrderByDescending(p => p.PublishedDate)
        .Take(5)
        .ToList();
}
```

### 4. Tăng View Count Cho Post

```csharp
var post = db.Posts.Find(postId);
if (post != null)
{
    post.ViewCount++;
    db.SaveChanges();
}
```

## Database Views & Stored Procedures

### Views

**vw_ActiveBannersByPosition**
```sql
-- Lấy banners đang active theo vị trí
SELECT * FROM vw_ActiveBannersByPosition
```

**vw_FeaturedPosts**
```sql
-- Lấy bài viết nổi bật
SELECT * FROM vw_FeaturedPosts
```

### Stored Procedures

**sp_GetBannersByPosition**
```sql
-- Lấy banners theo vị trí
EXEC sp_GetBannersByPosition @Position = 'Home'
```

**sp_GetRecentPosts**
```sql
-- Lấy bài viết mới nhất
EXEC sp_GetRecentPosts @TopCount = 10
```

## Tích Hợp Với Frontend

### Hiển Thị Hero Banner Trong View

```cshtml
@{
    using (var db = new FiveGuysProductEntities4())
    {
        var banners = db.HeroBanners
            .Where(b => b.BannerPosition == "Home" && b.IsCurrentlyActive)
            .OrderBy(b => b.DisplayOrder)
            .ToList();

        foreach (var banner in banners)
        {
            <div class="hero-banner" style="background-color: @banner.BackgroundColor">
                <img src="@banner.ImagePath" alt="@banner.Title" />
                <h1 style="color: @banner.TextColor">@banner.Title</h1>
                <p>@banner.Subtitle</p>
                @if (!string.IsNullOrEmpty(banner.ButtonText))
                {
                    <a href="@banner.ButtonLink" class="btn btn-primary">
                        @banner.ButtonText
                    </a>
                }
            </div>
        }
    }
}
```

### Hiển Thị Featured Posts

```cshtml
@{
    using (var db = new FiveGuysProductEntities4())
    {
        var posts = db.Posts
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderByDescending(p => p.PublishedDate)
            .Take(3)
            .ToList();

        foreach (var post in posts)
        {
            <article class="post-card">
                <img src="@post.ThumbnailImage" alt="@post.Title" />
                <h3>@post.Title</h3>
                <p>@post.ShortDescription</p>
                <div class="meta">
                    <span>By @post.Author</span>
                    <span>@post.ViewCount views</span>
                    <span>@post.PublishedDate.ToString("dd/MM/yyyy")</span>
                </div>
                <a href="/Posts/Details/@post.Id">Đọc thêm</a>
            </article>
        }
    }
}
```

## Security & Best Practices

### File Upload Security

✅ **Validation được implement**:
- Kiểm tra extension file
- Giới hạn kích thước file (5MB)
- Generate unique filename để tránh conflict
- Không cho phép overwrite file

### Input Validation

✅ **ModelState Validation**:
- Required fields
- String length limits
- Data annotations
- Anti-forgery tokens

### Error Handling

✅ **Try-Catch blocks** trong tất cả operations
✅ **User-friendly error messages**
✅ **TempData** cho success/error notifications

## Troubleshooting

### Lỗi: "File not found" khi upload

**Giải pháp**:
1. Kiểm tra quyền ghi vào thư mục `/images/uploads/`
2. Đảm bảo thư mục đã được tạo
3. Kiểm tra IIS Application Pool identity có quyền ghi

### Lỗi: "Connection string not found"

**Giải pháp**:
1. Kiểm tra `Web.config` có connection string `FiveGuysProductEntities4`
2. Đảm bảo SQL Server đang chạy
3. Test connection string bằng Server Explorer trong Visual Studio

### Lỗi: "Table not found"

**Giải pháp**:
1. Chạy lại SQL script `Database-ImageCRUD-Schema.sql`
2. Refresh database trong SSMS
3. Kiểm tra database name trong connection string

### Hình ảnh không hiển thị

**Giải pháp**:
1. Kiểm tra đường dẫn trong `ImagePath` có đúng không
2. Đảm bảo file tồn tại trong thư mục `/images/`
3. Check browser console cho errors
4. Kiểm tra IIS MIME types cho extensions

## Performance Optimization

### Database Indexes

✅ Indexes đã được tạo cho:
- `ImageType` in MediaImages
- `BannerPosition` in HeroBanners
- `IsActive` in all tables
- `Slug` in Posts (unique)
- `PublishedDate` in Posts

### Caching (Khuyến nghị)

Để tăng hiệu suất, nên implement caching cho:
```csharp
// Cache banners
var banners = System.Runtime.Caching.MemoryCache.Default["HomeBanners"]
    as List<HeroBanner>;

if (banners == null)
{
    using (var db = new FiveGuysProductEntities4())
    {
        banners = db.HeroBanners
            .Where(b => b.BannerPosition == "Home" && b.IsActive)
            .ToList();

        System.Runtime.Caching.MemoryCache.Default.Add(
            "HomeBanners",
            banners,
            DateTimeOffset.Now.AddMinutes(30)
        );
    }
}
```

## Future Enhancements (Đề xuất)

🔮 **Tính năng có thể thêm**:
- [ ] Image resizing/cropping tự động
- [ ] Multiple image upload
- [ ] Drag & drop upload
- [ ] Image gallery/lightbox
- [ ] CDN integration
- [ ] Image optimization (compress)
- [ ] Watermark for images
- [ ] Multi-language support
- [ ] Advanced search & filters
- [ ] Batch operations (delete, update)
- [ ] Export/Import functionality
- [ ] Image analytics/statistics
- [ ] Role-based access control

## Support & Contact

Nếu gặp vấn đề hoặc cần hỗ trợ:
1. Kiểm tra phần Troubleshooting ở trên
2. Review code trong Controllers và Models
3. Check SQL Server logs
4. Review IIS logs

## License

This project is part of FiveGuys Web Application - HUFLIT Course Project.

---

**Tạo bởi**: Claude AI Assistant
**Ngày tạo**: 06/01/2025
**Version**: 1.0.0
**Công nghệ**: ASP.NET MVC 5.2.9 + Entity Framework 6.4.4
