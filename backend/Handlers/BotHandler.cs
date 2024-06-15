
public interface IBotHandler {

}

public class BotHandler {

  public BotHandler() { }
  public IBot? GetBot(int levelID) {
    switch(levelID) {
      case 1:
        return new BeginnerBot();
      default:
        return null;
    }
  }
}

public interface IBot {
  public string Play(List<Match> matches);
} 

public class BeginnerBot : IBot {
  public string Play(List<Match> matches) {
    var winOutcomes = new Dictionary<string, string>() {
      {"rock", "paper"},
      {"paper", "scissors"},
      {"scissors", "rock"}
    };

    var match = matches.LastOrDefault();
    if (match != null) {
      // Winning outcomes vs. whatever they played last time
      return winOutcomes[match.PlayerChoice];
    }
    else {
      return "rock";
    }
  }
}