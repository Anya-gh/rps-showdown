public class MatchesWithChoice {
  public string Choice { get; set; }
  public List<Match> Matches { get; set; }

  public MatchesWithChoice(string choice, List<Match> matches) {
    Matches = matches;
    Choice = choice;
  }
}