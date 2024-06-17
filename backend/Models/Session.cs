public class Session {
  public int ID { get; set; }
  public required DateTime StartedAt { get; set; }
  public User User { get; set; } = null!;
  public required int UserID { get; set; }
  public Level Player { get; set; } = null!;
  public required int PlayerID { get; set; }
  public Level Level { get; set; } = null!;
  public required int LevelID { get; set; }
  public ICollection<Match> Matches { get; set; } = null!;
}