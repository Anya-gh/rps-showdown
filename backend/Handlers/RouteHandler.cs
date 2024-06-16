using System.Xml.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public interface IRouteHandler {

}

public class RouteHandler {

  private SecurityHandler SecurityHandler { get; set; }

  public RouteHandler(SecurityHandler securityHandler) {
    SecurityHandler = securityHandler;
  }
  public IResult Login(UserDetails user, RPSDbContext db) {
    if (SecurityHandler.UserExists(user, db)) {
      if (SecurityHandler.AuthenticateUser(user, db)) {
        // User exists and username password combination is valid
        return Results.Ok(SecurityHandler.CreateToken(user));
      }
      else { 
        // User exists but username password comination is not valid
        return Results.Unauthorized();
      }
    }
    else {
      // User does not exist, create user
      User newUser = new User { Username = user.Username, Password = user.Password };
      db.UserItems.Add(newUser);
      db.SaveChanges();
      return Results.Ok(SecurityHandler.CreateToken(user));
    }
  }

  public IResult Stats(StatsDetails user, RPSDbContext db) {

    ChoiceDistribution ChoiceDistribution(float rock, float paper, float scissors) {
      var total = Math.Max(rock + paper + scissors, 1);
      return new ChoiceDistribution {Rock = rock/total, Paper = paper/total, Scissors = scissors/total };
    }

    int? userID = (
      from userItem in db.UserItems
      where userItem.Username == user.Username
      select userItem.ID
    ).FirstOrDefault();

    if (userID < 1) { return Results.NotFound(); }

    List<Match> matches = (
      from matchItem in db.MatchItems
      where matchItem.UserID == userID && matchItem.PlayerID == -1
      orderby matchItem.LevelID
      select matchItem
    ).ToList();

    List<StatsInfo> statsInfo = new List<StatsInfo>();

    foreach (var levelItem in db.LevelItems) {
      if (levelItem.ID < 0) { continue; }
      int levelID = levelItem.ID;
      List<string> choices = new List<string>() { "rock", "paper", "scissors" };

      List<MatchesWithChoice> matchesWithChoice = choices.Select(choice => new MatchesWithChoice(choice, (
        from match in matches
        where match.LevelID == levelID && match.PlayerChoice == choice
        select match
      ).ToList())).ToList();

      List<ChoiceStat> timesUsed = matchesWithChoice.Select(choice => new ChoiceStat(choice.Choice, choice.Matches.Count())).ToList();

      var rockStat = timesUsed.Where(choice => choice.Choice == "rock").FirstOrDefault();
      float rockUsed = rockStat == null ? 0 : rockStat.Stat;
      var paperStat = timesUsed.Where(choice => choice.Choice == "paper").FirstOrDefault();
      float paperUsed = paperStat == null ? 0 : paperStat.Stat;
      var scissorsStat = timesUsed.Where(choice => choice.Choice == "scissors").FirstOrDefault();
      float scissorsUsed = scissorsStat == null ? 0 : scissorsStat.Stat;

      ChoiceDistribution choiceDistribution = ChoiceDistribution(
        rockUsed,
        paperUsed,
        scissorsUsed
      );

      var winningMatchesWithChoice = choices.Select(choice => new MatchesWithChoice(choice, (
        from match in matches
        where match.LevelID == levelID && match.PlayerChoice == choice && match.Result == "win"
        select match
      ).ToList())).ToList();

      List<ChoiceStat> winsWithChoice = winningMatchesWithChoice.Select(choice => new ChoiceStat(choice.Choice, choice.Matches.Count())).ToList();

      float total = timesUsed.Sum(choice => (int)choice.Stat);
      float wins = winsWithChoice.Sum(choice => (int)choice.Stat);
      float winRate = wins / Math.Max(total, 1);

      List<ChoiceStat> winRateWithChoice = (
        from winChoice in winsWithChoice
        join totalChoice in timesUsed on winChoice.Choice equals totalChoice.Choice
        select new ChoiceStat(winChoice.Choice, winChoice.Stat / Math.Max(totalChoice.Stat, 1))
      ).ToList();

      ChoiceStat? aceChoiceStat = winRateWithChoice.MaxBy(choice => choice.Stat);
      string ace = "none";
      if (aceChoiceStat != null && aceChoiceStat.Stat != 0) { ace = aceChoiceStat.Choice; }

      var losingMatchesAgainstChoice = choices.Select(choice => new MatchesWithChoice(choice, (
        from match in matches
        where match.LevelID == levelID && match.BotChoice == choice && match.Result == "lose"
        select match
      ).ToList())).ToList();

      List<ChoiceStat> lossesAgainstChoice = losingMatchesAgainstChoice.Select(choice => new ChoiceStat(choice.Choice, choice.Matches.Count())).ToList();

      List<ChoiceStat> loseRateAgainstChoice = (
        from winChoice in lossesAgainstChoice
        join totalChoice in timesUsed on winChoice.Choice equals totalChoice.Choice
        select new ChoiceStat(winChoice.Choice, winChoice.Stat / Math.Max(totalChoice.Stat, 1))
      ).ToList();
      
      ChoiceStat? nemesisChoiceStat = loseRateAgainstChoice.MaxBy(choice => choice.Stat);
      string nemesis = "none";
      if (nemesisChoiceStat != null && nemesisChoiceStat.Stat != 0) { nemesis = nemesisChoiceStat.Choice; }

      /* --- longest streak --- */

      var sequentialMatches = matches.Where(match => match.LevelID == levelID).OrderBy(match => match.ID)
      .Select((match, index) => new SequentialMatch {
        SessionID = match.SessionID,
        MatchNumber = index + 1,
        IsWin = match.Result == "win" ? 1 : 0
      });

      var winGroups = sequentialMatches.OrderBy(match => match.MatchNumber)
      .Select(match => new WinGroup {
        sequentialMatch = match,
        GroupID = match.MatchNumber - sequentialMatches.Take(match.MatchNumber).Sum(match => match.IsWin)
      })
      .Where(match => match.sequentialMatch.IsWin == 1);

      var winStreaks = winGroups.GroupBy(winGroup => (winGroup.sequentialMatch.SessionID, winGroup.GroupID)).Select(group => group.Count()).ToList();
      var longestStreak = winStreaks.Count() > 0 ? winStreaks.Max() : 0;

      StatsInfo levelStatsInfo = new StatsInfo { Ace = ace, Nemesis = nemesis, ChoiceDistribution = choiceDistribution, LevelID = levelID, LongestStreak = longestStreak, Style = "none", WinRate = winRate };

      statsInfo.Add(
        levelStatsInfo
      );
    }

    return Results.Ok(statsInfo);
  }

  public IResult GetPlayInfo(RPSDbContext db) {
    List<PlayInfo> playInfo = (
      from levelItem in db.LevelItems
      select new PlayInfo(levelItem.ID, levelItem.Name)
    ).ToList();
    return Results.Ok(playInfo);
  }

  public IResult CreateSession(SessionDetails session, RPSDbContext db) {
    int userID = (
      from userItem in db.UserItems
      where userItem.Username == session.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID < 1) { return Results.NotFound(); }
    DateTime startedAt = DateTime.UtcNow;
    Session newSession = new Session { UserID = userID, StartedAt = startedAt, LevelID = session.LevelID };
    try { 
      db.SessionItems.Add(newSession);
      db.SaveChanges();
      int sessionID = db.SessionItems.Where(session => session.UserID == userID && session.StartedAt == startedAt).Select(session => session.ID).FirstOrDefault();
      if (sessionID < 1) { return Results.StatusCode(500); }
      return Results.Ok(sessionID);
    }
    catch (Exception ex) {
      // There should be a separate catch for the case of not found levelID but this is fine for now/for these purposes
      Console.WriteLine(ex.Message);
      return Results.StatusCode(500);
    }
  }

  public IResult Play(PlayDetails play, RPSDbContext db) {

    int userID = (
      from userItem in db.UserItems
      where userItem.Username == play.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID < 1) { return Results.NotFound(); }

    int levelID = (
      from sessionItem in db.SessionItems
      where sessionItem.ID == play.SessionID
      select sessionItem.LevelID
    ).FirstOrDefault();
    if (levelID < 1) { return Results.NotFound(); }

    List<Match> matches = (
      from matchItem in db.MatchItems
      where matchItem.SessionID == play.SessionID && matchItem.PlayerID == -1
      select matchItem
    ).ToList();

    BotHandler botHandler = new BotHandler();
    IBot? Bot = botHandler.GetBot(levelID);
    if (Bot == null) { return Results.NotFound(); }

    string BotChoice = Bot.Play(matches);
    var outcomes = new Dictionary<(string, string), string>() {
      {("rock", "rock"), "draw"},
      {("rock", "paper"), "lose"},
      {("rock", "scissors"), "win"},
      {("paper", "rock"), "win"},
      {("paper", "paper"), "draw"},
      {("paper", "scissors"), "lose"},
      {("scissors", "rock"), "lose"},
      {("scissors", "paper"), "win"},
      {("scissors", "scissors"), "draw"}
    };
    string outcome = outcomes[(play.PlayerChoice, BotChoice)];

    Match newMatch = new Match { PlayerChoice = play.PlayerChoice, BotChoice = BotChoice, Result = outcome, PlayerID = -1, LevelID = levelID, SessionID = play.SessionID, UserID = userID };
    db.MatchItems.Add(newMatch);
    db.SaveChanges();

    PlayResponse playResponse = new PlayResponse(BotChoice, outcome);
    return Results.Ok(playResponse);
  }

  public IResult ValidUser() {
    return Results.Ok();
  }
}