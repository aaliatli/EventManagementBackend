using UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEvent>().HasKey(ue => new { ue.UserId, ue.EventId });
        modelBuilder.Entity<UserEvent>().HasOne(ue => ue.User).WithMany(u => u.UserEvents).HasForeignKey(ue => ue.UserId);
    }
}