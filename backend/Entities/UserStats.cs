public class UserStats {
  public int ID { get; set; }
  public float WinsBeginner { get; set; }
  public float DrawsBeginner { get; set; }
  public float LossesBeginner { get; set; }
  public float WinsIntermediate { get; set; }
  public float DrawsIntermediate { get; set; }
  public float LossesIntermediate { get; set; }
  public float WinsAdvanced { get; set; }
  public float DrawsAdvanced { get; set; }
  public float LossesAdvanced { get; set; }
  public string AceChoice { get; set; } = "";
  public string NemesisChoice { get; set; } = "";
  public int LongestStreak { get; set; }
  public string PlayStyle { get; set; } = "";
  public required User User { get; set; }
  public int UserID { get; set; }
}