public class Match {
  public int ID { get; set; }
  public string PlayerChoice { get; set; } = "";
  public string AIChoice { get; set; } = "";
  public string Result { get; set; }
  public Level Level { get; set; }
  public int LevelID { get; set; }
  public User User { get; set; }
  public int UserID { get; set; }
  public Session Session { get; set; }
  public int SessionID { get; set; }
}