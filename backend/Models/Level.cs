public class Level {
  public int ID { get; set; }
  public required string Name { get; set; }
  public ICollection<Session> PlayerSessions { get; set; } = null!;
  public ICollection<Session> LevelSessions { get; set; } = null!;
}