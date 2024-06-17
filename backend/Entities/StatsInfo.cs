using Microsoft.IdentityModel.Tokens;
public class StatsInfo {
  public float WinRate { get; set; }
  public int LongestStreak { get; set; }
  public ChoiceDistribution ChoiceDistribution { get; set; }
  public string Ace { get; set; }
  public string Nemesis { get; set; }
  public Playstyle Playstyle { get; set; }
  public int LevelID { get; set; }
}

public class ChoiceDistribution {
  public float Rock { get; set; }
  public float Paper { get; set; }
  public float Scissors { get; set; }
}

