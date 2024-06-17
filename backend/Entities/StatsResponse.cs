public class StatsResponse {
  public required float WinRate { get; set; }
  public required int LongestStreak { get; set; }
  public required ChoiceDistribution ChoiceDistribution { get; set; }
  public required string Ace { get; set; }
  public required string Nemesis { get; set; }
  public required Playstyle Playstyle { get; set; }
  public required int Games { get; set; }
  public required int LevelID { get; set; }
}

public class ChoiceDistribution {
  public required float Rock { get; set; }
  public required float Paper { get; set; }
  public required float Scissors { get; set; }
}

