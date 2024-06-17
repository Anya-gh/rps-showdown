public class StatsHandler() {
  public ChoiceDistribution GetChoiceDistribution(List<ChoiceStat> timesUsed) {

    var rockStat = timesUsed.Where(choice => choice.Choice == "rock").FirstOrDefault();
    float rockUsed = rockStat == null ? 0 : rockStat.Stat;
    var paperStat = timesUsed.Where(choice => choice.Choice == "paper").FirstOrDefault();
    float paperUsed = paperStat == null ? 0 : paperStat.Stat;
    var scissorsStat = timesUsed.Where(choice => choice.Choice == "scissors").FirstOrDefault();
    float scissorsUsed = scissorsStat == null ? 0 : scissorsStat.Stat;

    var total = Math.Max(rockUsed + paperUsed + scissorsUsed, 1);
    return new ChoiceDistribution {Rock = rockUsed/total, Paper = paperUsed/total, Scissors = scissorsUsed/total };
  }

  public string GetAce(List<Match> matches, List<ChoiceStat> timesUsed) {
    List<string> choices = new List<string>() { "rock", "paper", "scissors" };

    var winningMatchesWithChoice = choices.Select(choice => new MatchesWithChoice{ Choice = choice, Matches = (
      from match in matches
      where match.PlayerChoice == choice && match.Result == "win"
      select match
    ).ToList() }).ToList();

    List<ChoiceStat> winsWithChoice = winningMatchesWithChoice.Select(choice => new ChoiceStat { Choice = choice.Choice, Stat = choice.Matches.Count() }).ToList();

    List<ChoiceStat> winRateWithChoice = (
      from winChoice in winsWithChoice
      join totalChoice in timesUsed on winChoice.Choice equals totalChoice.Choice
      select new ChoiceStat { Choice = winChoice.Choice, Stat = winChoice.Stat / Math.Max(totalChoice.Stat, 1) }
    ).ToList();

    ChoiceStat? aceChoiceStat = winRateWithChoice.MaxBy(choice => choice.Stat);
    string ace = "none";
    if (aceChoiceStat != null && aceChoiceStat.Stat != 0) { ace = aceChoiceStat.Choice; }
    return ace;
  }

  public string GetNemesis(List<Match> matches, List<ChoiceStat> timesUsed) {
    List<string> choices = new List<string>() { "rock", "paper", "scissors" };

    var losingMatchesAgainstChoice = choices.Select(choice => new MatchesWithChoice { Choice = choice, Matches = (
        from match in matches
        where match.LevelChoice == choice && match.Result == "lose"
        select match
      ).ToList() }).ToList();

      List<ChoiceStat> lossesAgainstChoice = losingMatchesAgainstChoice.Select(choice => new ChoiceStat { Choice = choice.Choice, Stat = choice.Matches.Count() }).ToList();

      List<ChoiceStat> loseRateAgainstChoice = (
        from winChoice in lossesAgainstChoice
        join totalChoice in timesUsed on winChoice.Choice equals totalChoice.Choice
        select new ChoiceStat{ Choice = winChoice.Choice, Stat =  winChoice.Stat / Math.Max(totalChoice.Stat, 1) }
      ).ToList();
      
      ChoiceStat? nemesisChoiceStat = loseRateAgainstChoice.MaxBy(choice => choice.Stat);
      string nemesis = "none";
      if (nemesisChoiceStat != null && nemesisChoiceStat.Stat != 0) { nemesis = nemesisChoiceStat.Choice; }
      return nemesis;
  }

  public int GetLongestWinStreak(List<Match> matches) {
    // Give each match a sequential ID and 1 if it was a win, 0 if it was a loss
    var sequentialMatches = matches.OrderBy(match => match.ID)
      .Select((match, index) => new SequentialMatch {
        SessionID = match.SessionID,
        MatchNumber = index + 1,
        IsWin = match.Result == "win" ? 1 : 0
      });

      // Use cumulative wins up to a given match against sequential ID to check if they belong to the same streak of wins
      var winGroups = sequentialMatches.OrderBy(match => match.MatchNumber)
      .Select(match => new WinGroup {
        sequentialMatch = match,
        GroupID = match.MatchNumber - sequentialMatches.Take(match.MatchNumber).Sum(match => match.IsWin)
      })
      .Where(match => match.sequentialMatch.IsWin == 1);

      // Count wins grouped in streaks
      var winStreaks = winGroups.GroupBy(winGroup => (winGroup.sequentialMatch.SessionID, winGroup.GroupID)).Select(group => group.Count()).ToList();
      var longestStreak = winStreaks.Count() > 0 ? winStreaks.Max() : 0;
      return longestStreak;
  }
}