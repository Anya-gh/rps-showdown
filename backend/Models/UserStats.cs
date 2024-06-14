public class UserStats {
  public int ID { get; set; }
  public float Wins { get; set; }
  public float Draws { get; set; }
  public float Losses { get; set; }
  public string Ace { get; set; } = "";
  public string Nemesis { get; set; } = "";
  public int LongestStreak { get; set; }
  public string PlayStyle { get; set; } = "";
  public User User { get; set; }
  public int UserID { get; set; }
  public Level Level { get; set; }
  public int LeveLID { get; set; }
}