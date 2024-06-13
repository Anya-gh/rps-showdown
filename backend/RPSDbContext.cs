using Microsoft.EntityFrameworkCore;
public class RPSDbContext : DbContext {
  public RPSDbContext(DbContextOptions options) : base(options) { }
  public virtual DbSet<User> Users { get; set; }
  public virtual DbSet<Record> Records { get; set; }
  public virtual DbSet<UserStats> UserStats { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>(entity => {
          entity.HasMany(u => u.Records).WithOne(r => r.User).HasForeignKey(r => r.UserID);
          entity.HasOne(u => u.UserStats).WithOne(us => us.User).HasForeignKey<UserStats>(us => us.UserID); // need to explicitly type because it's not clear which has the foreign key to .net
        });
    }
}