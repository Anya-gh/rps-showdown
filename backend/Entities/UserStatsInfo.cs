using Microsoft.IdentityModel.Tokens;
public class UserStatsInfo {
  public float WinRate { get; set; }
  public int LongestStreak { get; set; }
  public (float, float, float) ChoiceDistribution { get; set; }
  public string Ace { get; set; }
  public string Nemesis { get; set; }
  public string Style { get; set; }
  public string Level { get; set; }

  public UserStatsInfo(float winRate, int longestStreak, (float, float, float) choiceDistribution, string ace, string nemesis, string style, string level) {
    WinRate = winRate;
    LongestStreak = longestStreak;
    ChoiceDistribution = choiceDistribution;
    Ace = ace;
    Nemesis = nemesis;
    Style = style;
    Level = level;
  }
}
public class UserStatsJoinLevel {
  public UserStats UserStats { get; set; }
  public Level Level { get; set; }
  public UserStatsJoinLevel(UserStats userStats, Level level) {
    UserStats = userStats;
    Level = level;
  }
}

