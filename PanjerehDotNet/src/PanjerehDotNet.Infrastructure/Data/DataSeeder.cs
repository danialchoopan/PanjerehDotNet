using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Infrastructure.Persistence;
namespace PanjerehDotNet.Infrastructure.Data;
public static class DataSeeder {
    public static async Task SeedAsync(ApplicationDbContext context) {
        if (context.Users.Any()) return;
        var admin = new User { PhoneNumber = "09120000000", FullName = "مدیر سیستم", IsAdmin = true };
        var user1 = new User { PhoneNumber = "09121111111", FullName = "علی رضایی" };
        var user2 = new User { PhoneNumber = "09122222222", FullName = "سارا احمدی" };
        await context.Users.AddRangeAsync(admin, user1, user2);
        var cat1 = new Category { Name = "املاک" };
        var cat2 = new Category { Name = "وسایل نقلیه" };
        var cat3 = new Category { Name = "کالای دیجیتال" };
        await context.Categories.AddRangeAsync(cat1, cat2, cat3);
        await context.SaveChangesAsync();
        var ad1 = new Advertisement { Title = "آگهی ۱", Description = "توضیح ۱", Price = 100, City = "تهران", District = "سعادت آباد", CategoryId = cat1.Id, UserId = user1.Id, IsApproved = true };
        await context.Advertisements.AddAsync(ad1);
        await context.SaveChangesAsync();
    }
}
