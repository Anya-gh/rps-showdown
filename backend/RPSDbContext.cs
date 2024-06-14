using Microsoft.EntityFrameworkCore;
public class RPSDbContext : DbContext {
  public RPSDbContext(DbContextOptions options) : base(options) { }
  public virtual DbSet<User> UserItems { get; set; }
  public virtual DbSet<Match> MatchItems { get; set; }
  public virtual DbSet<UserStats> UserStatsItems { get; set; }
  public virtual DbSet<Level> LevelItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>(entity => {
          entity.HasMany(u => u.Matches).WithOne(m => m.User).HasForeignKey(m => m.UserID);
          entity.HasOne(u => u.UserStats).WithOne(us => us.User).HasForeignKey<UserStats>(us => us.UserID); // need to explicitly type because it's not clear which has the foreign key to .net
        });
        modelBuilder.Entity<UserStats>(entity => {
          entity.HasOne(us => us.Level).WithMany(l => l.UserStats).HasForeignKey(us => us.LeveLID);
        });
        modelBuilder.Entity<Match>(entity => {
          entity.HasOne(m => m.Level).WithMany(l => l.Matches).HasForeignKey(m => m.LevelID);
        });
        modelBuilder.Entity<Level>().HasData(
          new Level { ID = 1, Name = "Beginner" },
          new Level { ID = 2, Name = "Intermediate" },
          new Level { ID = 3, Name = "Advanced" }
        );
    }
}