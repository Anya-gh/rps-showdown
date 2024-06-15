using System.Numerics;
using MathNet.Numerics.LinearAlgebra;

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
      case 3:
        return new AdvancedBot();
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
If it was a draw: picks a choice at random.
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
/*
Uses a Markov chain to predict player's next choice.
To do so it constructs a transition matrix, which looks like this:
  [(R->R), (R->P), (R->S),
   (P->R), (P->P), (P->S),
   (S->R), (S->P), (S->S)]
where X->Y is the probability of choosing Y after just choosing X.
To get the probability of X->Y, it does:
no. of times X->Y / no. of times X
Then, if we now take X to be the player's previous choice, it
finds what the player is most likely to do by picking the Y with
the greatest X->Y probability; the choice they're most likely to
pick given they just picked X.
*/
public class AdvancedBot : IBot {
    public override string Play(List<Match> matches)
    {
      // Need at least one sequence to observe
      if (matches.Count() < 2) { return Choices[random.Next(0, 2)]; }

      List<string> playerChoices = matches.Select(match => match.PlayerChoice).ToList();
      var transMatrix = new Dictionary<string, Dictionary<string, float>>();
      var transCounts = new Dictionary<string, Dictionary<string, int>>();
      var totalCounts = new Dictionary<string, int>();

      // Initialise transMatrix, transCounts and totalCounts to zeros
      foreach (string prevChoice in Choices) {
        totalCounts[prevChoice] = 0;
        transMatrix.Add(prevChoice, new Dictionary<string, float>());
        transCounts.Add(prevChoice, new Dictionary<string, int>());
        foreach (string nextChoice in Choices) {
          transMatrix[prevChoice].Add(nextChoice, 0.0f);
          transCounts[prevChoice].Add(nextChoice, 0);
        }
      }

      for (int i = 0; i < playerChoices.Count() - 1; i++) {
        string prevChoice = playerChoices[i];
        string nextChoice = playerChoices[i+1];
        transCounts[prevChoice][nextChoice] += 1;
        totalCounts[prevChoice] += 1;
      }


      foreach (string prevChoice in Choices) {
        foreach (string nextChoice in Choices) {
          transMatrix[prevChoice][nextChoice] = transCounts[prevChoice][nextChoice] / Math.Max(totalCounts[prevChoice], 1);
        }
      }

      Match lastMatch = matches.Last();
      var choicesByProbability = transMatrix[lastMatch.PlayerChoice];
      // This can only be null if PlayerChoice isn't in Choices, which shouldn't give a valid return anyway.
      string mostLikelyChoice = choicesByProbability.MaxBy(kvp => kvp.Value).Key!;
      return WinOutcomes[mostLikelyChoice];
    }
}