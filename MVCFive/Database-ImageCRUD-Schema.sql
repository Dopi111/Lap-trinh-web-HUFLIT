-- ========================================
-- FiveGuys - Image Management CRUD System
-- Database Schema Script
-- ========================================
-- Tạo các bảng để quản lý hình ảnh, banners và bài viết
-- Chạy script này trong SQL Server Management Studio
-- ========================================

USE [FiveGuysProduct]
GO

-- ========================================
-- 1. Tạo bảng MediaImages
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaImages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MediaImages](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ImageName] [nvarchar](200) NOT NULL,
        [ImagePath] [nvarchar](500) NOT NULL,
        [Description] [nvarchar](1000) NULL,
        [ImageType] [nvarchar](50) NOT NULL,
        [FileSize] [bigint] NULL,
        [FileExtension] [nvarchar](10) NULL,
        [Width] [int] NULL,
        [Height] [int] NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime] NULL,
        [CreatedBy] [nvarchar](100) NULL,
     CONSTRAINT [PK_MediaImages] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Create indexes for MediaImages
CREATE NONCLUSTERED INDEX [IX_MediaImages_ImageType] ON [dbo].[MediaImages]([ImageType])
GO
CREATE NONCLUSTERED INDEX [IX_MediaImages_IsActive] ON [dbo].[MediaImages]([IsActive])
GO

-- ========================================
-- 2. Tạo bảng HeroBanners
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HeroBanners]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[HeroBanners](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [Subtitle] [nvarchar](500) NULL,
        [Description] [nvarchar](1000) NULL,
        [ImagePath] [nvarchar](500) NOT NULL,
        [MobileImagePath] [nvarchar](500) NULL,
        [ButtonText] [nvarchar](200) NULL,
        [ButtonLink] [nvarchar](500) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [BannerPosition] [nvarchar](50) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [StartDate] [datetime] NULL,
        [EndDate] [datetime] NULL,
        [BackgroundColor] [nvarchar](20) NULL,
        [TextColor] [nvarchar](20) NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime] NULL,
        [CreatedBy] [nvarchar](100) NULL,
     CONSTRAINT [PK_HeroBanners] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Create indexes for HeroBanners
CREATE NONCLUSTERED INDEX [IX_HeroBanners_BannerPosition] ON [dbo].[HeroBanners]([BannerPosition])
GO
CREATE NONCLUSTERED INDEX [IX_HeroBanners_IsActive] ON [dbo].[HeroBanners]([IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_HeroBanners_DisplayOrder] ON [dbo].[HeroBanners]([DisplayOrder])
GO

-- ========================================
-- 3. Tạo bảng Posts
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Posts](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](300) NOT NULL,
        [Slug] [nvarchar](300) NOT NULL,
        [ShortDescription] [nvarchar](500) NULL,
        [Content] [nvarchar](max) NOT NULL,
        [FeaturedImage] [nvarchar](500) NULL,
        [ThumbnailImage] [nvarchar](200) NULL,
        [CategoryId] [int] NULL,
        [Tags] [nvarchar](200) NULL,
        [ViewCount] [int] NOT NULL DEFAULT 0,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsFeatured] [bit] NOT NULL DEFAULT 0,
        [PublishedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime] NULL,
        [Author] [nvarchar](100) NULL,
        [CreatedBy] [nvarchar](100) NULL,
        [MetaTitle] [nvarchar](200) NULL,
        [MetaDescription] [nvarchar](500) NULL,
        [MetaKeywords] [nvarchar](200) NULL,
     CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
     CONSTRAINT [FK_Posts_Categories] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
    )
END
GO

-- Create indexes for Posts
CREATE UNIQUE NONCLUSTERED INDEX [IX_Posts_Slug] ON [dbo].[Posts]([Slug])
GO
CREATE NONCLUSTERED INDEX [IX_Posts_CategoryId] ON [dbo].[Posts]([CategoryId])
GO
CREATE NONCLUSTERED INDEX [IX_Posts_IsActive] ON [dbo].[Posts]([IsActive])
GO
CREATE NONCLUSTERED INDEX [IX_Posts_IsFeatured] ON [dbo].[Posts]([IsFeatured])
GO
CREATE NONCLUSTERED INDEX [IX_Posts_PublishedDate] ON [dbo].[Posts]([PublishedDate] DESC)
GO

-- ========================================
-- 4. Insert Sample Data
-- ========================================

-- Sample MediaImages
IF NOT EXISTS (SELECT * FROM MediaImages WHERE ImageType = 'Product')
BEGIN
    INSERT INTO [dbo].[MediaImages]
    ([ImageName], [ImagePath], [Description], [ImageType], [DisplayOrder], [IsActive], [CreatedBy])
    VALUES
    (N'Đường Hồ', N'/images/DuongHo.png', N'Hình ảnh sản phẩm Đường Hồ', N'Product', 1, 1, N'Admin'),
    (N'Latte', N'/images/LaiSua.png', N'Hình ảnh sản phẩm Latte', N'Product', 2, 1, N'Admin'),
    (N'Matcha', N'/images/Matcha.png', N'Hình ảnh sản phẩm Matcha', N'Product', 3, 1, N'Admin'),
    (N'Socola Đá Xay', N'/images/SocolaDaXay.png', N'Hình ảnh sản phẩm Socola', N'Product', 4, 1, N'Admin'),
    (N'Gift Box', N'/images/Gift.png', N'Hình ảnh quà tặng', N'Product', 5, 1, N'Admin')
END
GO

-- Sample HeroBanners
IF NOT EXISTS (SELECT * FROM HeroBanners WHERE BannerPosition = 'Home')
BEGIN
    INSERT INTO [dbo].[HeroBanners]
    ([Title], [Subtitle], [Description], [ImagePath], [ButtonText], [ButtonLink], [DisplayOrder], [BannerPosition], [IsActive], [CreatedBy])
    VALUES
    (N'Chào Mừng Đến Với FiveGuys',
     N'Trà Sữa Chất Lượng Cao',
     N'Trải nghiệm hương vị tuyệt vời với các sản phẩm trà sữa độc đáo của chúng tôi',
     N'/images/slider-img.png',
     N'Xem Menu',
     N'/Shop',
     1,
     N'Home',
     1,
     N'Admin'),

    (N'Ưu Đãi Đặc Biệt',
     N'Giảm giá 20% cho đơn hàng đầu tiên',
     N'Đăng ký ngay hôm nay để nhận ưu đãi hấp dẫn',
     N'/images/slider-bg.jpg',
     N'Đăng Ký Ngay',
     N'/Contact',
     2,
     N'Home',
     1,
     N'Admin')
END
GO

-- Sample Posts
IF NOT EXISTS (SELECT * FROM Posts WHERE Slug = 'gioi-thieu-fiveguys')
BEGIN
    DECLARE @CategoryId INT = (SELECT TOP 1 Id FROM Categories)

    INSERT INTO [dbo].[Posts]
    ([Title], [Slug], [ShortDescription], [Content], [FeaturedImage], [CategoryId], [Tags], [IsActive], [IsFeatured], [Author], [CreatedBy])
    VALUES
    (N'Giới Thiệu Về FiveGuys',
     N'gioi-thieu-fiveguys',
     N'Câu chuyện thành lập và phát triển của FiveGuys - Thương hiệu trà sữa uy tín',
     N'<h2>FiveGuys - Hương Vị Trà Sữa Đặc Biệt</h2>
     <p>FiveGuys là thương hiệu trà sữa được thành lập với mục tiêu mang đến những ly trà sữa chất lượng cao nhất cho khách hàng.</p>
     <p>Với đội ngũ barista chuyên nghiệp và nguyên liệu cao cấp, chúng tôi cam kết mang đến trải nghiệm tuyệt vời nhất.</p>',
     N'/images/agency-img.jpg',
     @CategoryId,
     N'Giới thiệu, FiveGuys, Trà sữa',
     1,
     1,
     N'Admin',
     N'Admin'),

    (N'Top 5 Món Trà Sữa Bán Chạy Nhất',
     N'top-5-mon-tra-sua-ban-chay-nhat',
     N'Khám phá 5 món trà sữa được yêu thích nhất tại FiveGuys',
     N'<h2>Top 5 Món Được Khách Hàng Yêu Thích</h2>
     <ol>
     <li><strong>Đường Hồ:</strong> Hương vị độc đáo với trà ô long</li>
     <li><strong>Matcha Latte:</strong> Trà xanh Nhật Bản nguyên chất</li>
     <li><strong>Socola Đá Xay:</strong> Mát lạnh, thơm ngon</li>
     <li><strong>Trà Sữa Truyền Thống:</strong> Hương vị quen thuộc</li>
     <li><strong>Latte:</strong> Cà phê sữa đậm đà</li>
     </ol>',
     N'/images/p1.png',
     @CategoryId,
     N'Top món, Best seller, Trà sữa',
     1,
     1,
     N'Admin',
     N'Admin')
END
GO

-- ========================================
-- 5. Create Views for Reports (Optional)
-- ========================================

-- View to get active banners by position
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ActiveBannersByPosition')
    DROP VIEW vw_ActiveBannersByPosition
GO

CREATE VIEW vw_ActiveBannersByPosition
AS
SELECT
    Id,
    Title,
    Subtitle,
    ImagePath,
    MobileImagePath,
    ButtonText,
    ButtonLink,
    BannerPosition,
    DisplayOrder,
    BackgroundColor,
    TextColor
FROM HeroBanners
WHERE IsActive = 1
  AND (StartDate IS NULL OR StartDate <= GETDATE())
  AND (EndDate IS NULL OR EndDate >= GETDATE())
GO

-- View to get featured posts
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_FeaturedPosts')
    DROP VIEW vw_FeaturedPosts
GO

CREATE VIEW vw_FeaturedPosts
AS
SELECT
    p.Id,
    p.Title,
    p.Slug,
    p.ShortDescription,
    p.FeaturedImage,
    p.ThumbnailImage,
    p.ViewCount,
    p.PublishedDate,
    p.Author,
    c.TenLoai as CategoryName
FROM Posts p
LEFT JOIN Categories c ON p.CategoryId = c.Id
WHERE p.IsActive = 1
  AND p.IsFeatured = 1
GO

-- ========================================
-- 6. Stored Procedures (Optional)
-- ========================================

-- Procedure to get banners by position
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetBannersByPosition')
    DROP PROCEDURE sp_GetBannersByPosition
GO

CREATE PROCEDURE sp_GetBannersByPosition
    @Position NVARCHAR(50)
AS
BEGIN
    SELECT *
    FROM HeroBanners
    WHERE BannerPosition = @Position
      AND IsActive = 1
      AND (StartDate IS NULL OR StartDate <= GETDATE())
      AND (EndDate IS NULL OR EndDate >= GETDATE())
    ORDER BY DisplayOrder
END
GO

-- Procedure to get recent posts
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetRecentPosts')
    DROP PROCEDURE sp_GetRecentPosts
GO

CREATE PROCEDURE sp_GetRecentPosts
    @TopCount INT = 10
AS
BEGIN
    SELECT TOP (@TopCount)
        p.*,
        c.TenLoai as CategoryName
    FROM Posts p
    LEFT JOIN Categories c ON p.CategoryId = c.Id
    WHERE p.IsActive = 1
    ORDER BY p.PublishedDate DESC
END
GO

-- ========================================
-- Script Completed Successfully
-- ========================================
PRINT 'Database schema for Image Management CRUD created successfully!'
PRINT 'Tables created: MediaImages, HeroBanners, Posts'
PRINT 'Sample data inserted'
PRINT 'Views and Stored Procedures created'
GO
