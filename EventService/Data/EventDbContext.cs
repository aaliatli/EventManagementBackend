using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }
    public DbSet<Event> Events { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEvent>().HasKey(ue => new { ue.UserId, ue.EventId });
        modelBuilder.Entity<UserEvent>().HasOne(ue => ue.Event).WithMany(u => u.UserEvents).HasForeignKey(ue => ue.EventId);
    }
}