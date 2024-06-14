using Microsoft.EntityFrameworkCore;
public class RPSDbContext : DbContext {
  public RPSDbContext(DbContextOptions options) : base(options) { }
  public virtual DbSet<User> UserItems { get; set; }
  public virtual DbSet<Match> MatchItems { get; set; }
  public virtual DbSet<UserStats> UserStatsItems { get; set; }
  public virtual DbSet<Level> LevelItems { get; set; }
  public virtual DbSet<Session> SessionItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>(entity => {
          entity.HasMany(u => u.Matches).WithOne(m => m.User).HasForeignKey(m => m.UserID);
          entity.HasMany(u => u.UserStats).WithOne(us => us.User).HasForeignKey(us => us.UserID);
          entity.HasMany(u => u.Sessions).WithOne(s => s.User).HasForeignKey(s => s.UserID);
        });
        modelBuilder.Entity<UserStats>(entity => {
          entity.HasKey(us => new {us.UserID, us.LeveLID}).HasName("PK_UserStatsItems");
          entity.HasOne(us => us.Level).WithMany(l => l.UserStats).HasForeignKey(us => us.LeveLID);
        });
        modelBuilder.Entity<Match>(entity => {
          entity.HasOne(m => m.Level).WithMany(l => l.Matches).HasForeignKey(m => m.LevelID);
          entity.HasOne(m => m.Session).WithMany(s => s.Matches).HasForeignKey(m => m.SessionID);
        });
        modelBuilder.Entity<Level>().HasData(
          new Level { ID = 1, Name = "Beginner" },
          new Level { ID = 2, Name = "Intermediate" },
          new Level { ID = 3, Name = "Advanced" }
        );
    }
}