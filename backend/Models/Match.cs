public class Match {
  public int ID { get; set; }
  public required string PlayerChoice { get; set; }
  public required string BotChoice { get; set; }
  public required string Result { get; set; }
  public User User { get; set; } = null!;
  public required int UserID { get; set; }
  public Session Session { get; set; } = null!;
  public required int SessionID { get; set; }
}