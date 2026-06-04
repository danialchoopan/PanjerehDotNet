using Microsoft.EntityFrameworkCore;
using PanjerehDotNet.Domain.Entities;
namespace PanjerehDotNet.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<AdImage> AdImages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ChatMessage>().HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ChatMessage>().HasOne(m => m.Receiver).WithMany().HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Advertisement>().HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        modelBuilder.Entity<Advertisement>().HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId);
    }
}
