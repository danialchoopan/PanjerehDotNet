using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Infrastructure.Persistence;
using System.Linq;

namespace PanjerehDotNet.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Users.Any()) return;

        var admin = new User { PhoneNumber = "09120000000", FullName = "مدیر سیستم", IsAdmin = true };
        var user1 = new User { PhoneNumber = "09121111111", FullName = "علی رضایی" };
        var user2 = new User { PhoneNumber = "09122222222", FullName = "سارا احمدی" };

        await context.Users.AddRangeAsync(admin, user1, user2);
        await context.SaveChangesAsync();

        var cat1 = new Category { Name = "املاک" };
        var cat2 = new Category { Name = "وسایل نقلیه" };
        var cat3 = new Category { Name = "کالای دیجیتال" };

        await context.Categories.AddRangeAsync(cat1, cat2, cat3);
        await context.SaveChangesAsync();

        var ad1 = new Advertisement
        {
            Title = "آپارتمان ۸۰ متری در سعادت آباد",
            Description = "بسیار تمیز، نورگیر عالی، دسترسی مناسب به مترو و مراکز خرید.",
            Price = 12000000000,
            City = "تهران",
            District = "سعادت آباد",
            Latitude = 35.78,
            Longitude = 51.37,
            CategoryId = cat1.Id,
            UserId = user1.Id,
            IsApproved = true,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };

        var ad2 = new Advertisement
        {
            Title = "پژو ۲۰۶ تیپ ۵ مدل ۹۸",
            Description = "بدون رنگ، فنی سالم، بیمه تا آخر سال، کم کارکرد.",
            Price = 450000000,
            City = "تهران",
            District = "پونک",
            Latitude = 35.76,
            Longitude = 51.33,
            CategoryId = cat2.Id,
            UserId = user2.Id,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        await context.Advertisements.AddRangeAsync(ad1, ad2);
        await context.SaveChangesAsync();

        var chat1 = new ChatMessage
        {
            Content = "سلام، آیا قیمت جای تخفیف دارد؟",
            SenderId = user2.Id,
            ReceiverId = user1.Id,
            AdvertisementId = ad1.Id,
            SentAt = DateTime.UtcNow.AddHours(-5)
        };

        await context.ChatMessages.AddAsync(chat1);

        var report1 = new AdReport
        {
            Reason = "قیمت نامناسب",
            Description = "قیمت این آگهی بسیار بالاتر از عرف بازار است.",
            AdvertisementId = ad1.Id,
            UserId = user2.Id,
            CreatedAt = DateTime.UtcNow
        };
        await context.Reports.AddAsync(report1);

        await context.SaveChangesAsync();
    }
}
