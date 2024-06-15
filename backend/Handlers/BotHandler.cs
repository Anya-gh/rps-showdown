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
  public Random random = new Random();
  public Dictionary<string, string> WinOutcomes = new Dictionary<string, string>() {
    {"rock", "paper"},
    {"paper", "scissors"},
    {"scissors", "rock"}
  };
} 
/*
Plays whatever won last time. Treats draw as a win.
If there hasn't been a match yet, picks a choice at random.
*/
public class BeginnerBot : IBot {
  public override string Play(List<Match> matches) {

    var lastMatch = matches.LastOrDefault();
    if (lastMatch != null) {
      if (lastMatch.Result == "win") { return lastMatch.PlayerChoice; }
      else { return lastMatch.BotChoice; }
    }
    else {
      return Choices[random.Next(0, 2)];
    }
  }
}
/*
Based on this paper: https://arxiv.org/pdf/1404.5199v1
If it just lost: switches to the choice that beats what the player just played.
If it just won: switches to the losing choice that the player just played.
If there hasn't been a match yet, picks a choice at random.
*/
public class IntermediateBot : IBot {
  public override string Play(List<Match> matches) {
    var lastMatch = matches.LastOrDefault();
    if (lastMatch != null) {
      // Player won
      if (lastMatch.Result == "win") {
        return WinOutcomes[lastMatch.PlayerChoice];
      }
      // Draw
      else if (lastMatch.Result == "draw") {
        return Choices[random.Next(0, 2)];
      }
      // Bot won
      else {
        return lastMatch.PlayerChoice;
      }
    }
    else {
      return Choices[random.Next(0, 2)];
    }
  }
}