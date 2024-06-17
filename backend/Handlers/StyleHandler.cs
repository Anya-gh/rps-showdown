using System.Diagnostics.Eventing.Reader;

public interface IStyleHandler {
  public Playstyle DetermineStyle(List<Match> matches);

}
public class StyleHandler : IStyleHandler {

  public Playstyle DetermineStyle(List<Match> matches) {
    if (matches.Count() == 0) { return GetStyle("none"); }
    float totalPivots = 0.0f;
    int totalLosses = 0;
    for (int i = 0; i < matches.Count() - 1; i++) {
      var prevMatch = matches[i];
      var nextMatch = matches[i+1];
      if (prevMatch.Result == "lose") { 
        totalLosses++;

        if (prevMatch.PlayerChoice != nextMatch.PlayerChoice) {
          totalPivots++;
        }
      }
    }
    if (totalLosses < 5) { return GetStyle("none"); }
    // If what they're doing isn't working and they stay their ground, they're passive
    else if (totalPivots / totalLosses <= 0.4) { return GetStyle("passive"); }
    // If what they're doing isn't working and they like to change it, they're aggressive
    else if (totalPivots / totalLosses >= 0.6) { return GetStyle("aggressive"); }
    // If they're somewhere in between they're balanced
    else { return GetStyle("balanced"); }
  }
  private Playstyle GetStyle(string style) {
    switch (style) {
      case "none":
        return new Playstyle { Style = "none", Description = "Play some more games to find out what your style is!" };
      case "aggressive":
        return new Playstyle { Style = "aggressive", Description = "You adapt fast, often reacting to what happened last time, and don't like losing the same way twice." };
      case "passive":
        return new Playstyle { Style = "passive", Description = "You keep your cool and stick to a plan. You don't like changing your mind." };
      case "balanced":
        return new Playstyle { Style = "balanced", Description = "You have a good balance of staying your ground and throwing off your opponent." };
      default:
        return new Playstyle { Style = "none", Description = "Play some more games to find out what your style is!" };
    }
  }
}