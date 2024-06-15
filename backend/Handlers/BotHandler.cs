using System.Diagnostics;

public interface IBotHandler {

}

public class BotHandler {

  public BotHandler() { }
  public IBot? GetBot(int levelID) {
    switch(levelID) {
      case 1:
        return new BeginnerBot();
      case 2:
        return new IntermediateBot(); 
      default:
        return null;
    }
  }
}

public abstract class IBot {
  public virtual string Play(List<Match> matches) { return "rock"; }
  public List<string> Choices = new List<string>() { "rock", "paper", "scissors" };
  public Dictionary<string, string> WinOutcomes = new Dictionary<string, string>() {
    {"rock", "paper"},
    {"paper", "scissors"},
    {"scissors", "rock"}
  };
} 

public class BeginnerBot : IBot {
  public override string Play(List<Match> matches) {

    var match = matches.LastOrDefault();
    if (match != null) {
      // Winning outcome vs. whatever they played last time
      return WinOutcomes[match.PlayerChoice];
    }
    else {
      return "rock";
    }
  }
}

public class IntermediateBot : IBot {
  public override string Play(List<Match> matches) {
    List<string> lastFewChoices = matches.TakeLast(Math.Min(matches.Count(), 3)).Select(match => match.PlayerChoice).ToList();
    if (lastFewChoices.Count() > 0) {
      // Winning outcome vs. whatever you played the most the last three times
      string mostCommonChoice = lastFewChoices.GroupBy(c => c).OrderByDescending(g => g.Count()).Select(c => c.Key).First();
      return WinOutcomes[mostCommonChoice]; 
    }
    else { 
      return "paper";
    }
  }
}