using System.Xml.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public interface IRouteHandler {
  public IResult Login(UserRequest user, RPSDbContext db);
  public IResult Stats(StatsRequest user, RPSDbContext db);
  public IResult GetPlayInfo(RPSDbContext db);
  public IResult CreateSession(SessionRequest session, RPSDbContext db);
  public IResult Play(PlayRequest play, RPSDbContext db);
  public IResult Spectate(SpectateRequest request, RPSDbContext db);
  public IResult ValidUser();
}

public class RouteHandler : IRouteHandler {

  private SecurityHandler SecurityHandler { get; set; }

  public RouteHandler(SecurityHandler securityHandler) {
    SecurityHandler = securityHandler;
  }
  public IResult Login(UserRequest user, RPSDbContext db) {
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

  public IResult Stats(StatsRequest user, RPSDbContext db) {

    int? userID = (
      from userItem in db.UserItems
      where userItem.Username == user.Username
      select userItem.ID
    ).FirstOrDefault();

    if (userID < 1) { return Results.NotFound(); }

    List<Match> matches = (
      from matchItem in db.MatchItems
      join sessionItem in db.SessionItems on matchItem.SessionID equals sessionItem.ID
      where matchItem.UserID == userID && sessionItem.PlayerID == -1
      orderby sessionItem.LevelID
      select matchItem
    ).ToList();

    List<StatsResponse> statsResponse = new List<StatsResponse>();

    foreach (var levelItem in db.LevelItems) {
      if (levelItem.ID < 0) { continue; }
      int levelID = levelItem.ID;
      List<string> choices = new List<string>() { "rock", "paper", "scissors" };

      List<Match> currentLevelMatches = (
        from match in matches
        join sessionItem in db.SessionItems on match.SessionID equals sessionItem.ID
        where sessionItem.LevelID == levelID
        select match
      ).ToList();

      StatsHandler statsHandler = new StatsHandler();

      List<MatchesWithChoice> matchesWithChoice = choices.Select(choice => new MatchesWithChoice{ Choice = choice, Matches = (
        from match in currentLevelMatches
        where match.PlayerChoice == choice
        select match
      ).ToList() }).ToList();

      List<ChoiceStat> timesUsed = matchesWithChoice.Select(choice => new ChoiceStat { Choice = choice.Choice, Stat = choice.Matches.Count() }).ToList();

      ChoiceDistribution choiceDistribution = statsHandler.GetChoiceDistribution(timesUsed);

      string ace = statsHandler.GetAce(currentLevelMatches, timesUsed);

      string nemesis = statsHandler.GetNemesis(currentLevelMatches, timesUsed);

      int longestStreak = statsHandler.GetLongestWinStreak(currentLevelMatches);

      // Playstyle
      StyleHandler styleHandler = new StyleHandler();
      Playstyle playstyle = styleHandler.DetermineStyle(currentLevelMatches);

      // Games and Win Rate
      int games = currentLevelMatches.Where(match => match.Result != "draw").Count();
      float wins = currentLevelMatches.Where(match => match.Result == "win").Count();
      float winRate = wins / Math.Max(games, 1.0f);
      
      StatsResponse levelStatsResponse = new StatsResponse { Ace = ace, Nemesis = nemesis, ChoiceDistribution = choiceDistribution, LevelID = levelID, LongestStreak = longestStreak, Playstyle = playstyle, WinRate = winRate, Games = games };

      statsResponse.Add(
        levelStatsResponse
      );
    }

    return Results.Ok(statsResponse);
  }

  public IResult GetPlayInfo(RPSDbContext db) {
    List<PlayInfo> playInfo = (
      from levelItem in db.LevelItems
      select new PlayInfo { LevelID = levelItem.ID, Name = levelItem.Name }
    ).ToList();
    return Results.Ok(playInfo);
  }

  public IResult CreateSession(SessionRequest session, RPSDbContext db) {
    if (session.PlayerID == session.LevelID) { return Results.BadRequest("The two players in one session cannot be the same."); }
    
    int userID = (
      from userItem in db.UserItems
      where userItem.Username == session.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID == 0) { return Results.NotFound(); }

    int levelID = (
      from levelItem in db.LevelItems
      where levelItem.ID == session.LevelID
      select levelItem.ID
    ).FirstOrDefault();
    if (levelID == 0) { return Results.BadRequest("This level does not exist."); }

    int playerID = (
      from levelItem in db.LevelItems
      where levelItem.ID == session.PlayerID
      select levelItem.ID
    ).FirstOrDefault();
    if (playerID == 0) { return Results.BadRequest("This level does not exist."); }

    DateTime startedAt = DateTime.UtcNow;
    Session newSession = new Session { UserID = userID, StartedAt = startedAt, PlayerID = session.PlayerID, LevelID = session.LevelID };
    db.SessionItems.Add(newSession);
    db.SaveChanges();
    int sessionID = db.SessionItems.Where(session => session.UserID == userID && session.StartedAt == startedAt).Select(session => session.ID).FirstOrDefault();
    return Results.Ok(sessionID);
  }

  public IResult Play(PlayRequest play, RPSDbContext db) {

    if (play.PlayerChoice != "rock" && play.PlayerChoice != "paper" && play.PlayerChoice != "scissors") {
      return Results.BadRequest("Invalid choice.");
    }

    int userID = (
      from userItem in db.UserItems
      where userItem.Username == play.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID < 1) { return Results.NotFound("Username not found."); }

    int levelID = (
      from sessionItem in db.SessionItems
      where sessionItem.ID == play.SessionID
      select sessionItem.LevelID
    ).FirstOrDefault();
    if (levelID < 1) { return Results.NotFound("Bot level not found."); }

    List<Match> matches = (
      from matchItem in db.MatchItems
      where matchItem.SessionID == play.SessionID
      select matchItem
    ).ToList();

    BotHandler botHandler = new BotHandler();
    IBot? Bot = botHandler.GetBot(levelID);
    if (Bot == null) { return Results.StatusCode(500); }

    string LevelChoice = Bot.Play(matches);

    string outcome = botHandler.DetermineResult(play.PlayerChoice, LevelChoice);

    Match newMatch = new Match { PlayerChoice = play.PlayerChoice, LevelChoice = LevelChoice, Result = outcome, SessionID = play.SessionID, UserID = userID };
    db.MatchItems.Add(newMatch);
    db.SaveChanges();

    PlayResponse playResponse = new PlayResponse { LevelChoice = LevelChoice, Result = outcome };
    return Results.Ok(playResponse);
  }

  public IResult Spectate(SpectateRequest request, RPSDbContext db) {
    int userID = (
      from userItem in db.UserItems
      where userItem.Username == request.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID < 1) { return Results.NotFound("Username not found."); }

    int levelID = (
      from sessionItem in db.SessionItems
      where sessionItem.ID == request.SessionID
      select sessionItem.LevelID
    ).FirstOrDefault();
    if (levelID == 0) { return Results.NotFound("Session does not exist."); }
    if (levelID < 0) { return Results.BadRequest("Cannot spectate session unless both players are bots."); }

    int playerID = (
      from sessionItem in db.SessionItems
      where sessionItem.ID == request.SessionID
      select sessionItem.PlayerID
    ).FirstOrDefault();

    if (playerID == 0) { return Results.NotFound("Session does not exist."); }
    if (playerID < 0) { return Results.BadRequest("Cannot spectate session unless both players are bots."); }

    List<Match> playerMatches = (
      from matchItem in db.MatchItems
      where matchItem.SessionID == request.SessionID
      select matchItem
    ).ToList();


    string FlipResult(string result) {
      if (result == "win") { return "lose"; }
      else if (result == "lose" ) { return "win"; }
      else { return "draw"; }
    }

    // Flip PlayerChoice and LevelChoice to make it look like the other bot is the opponent
    List<Match> levelMatches = playerMatches.Select(matchItem => new Match { 
      ID = matchItem.ID, SessionID = matchItem.ID, UserID = matchItem.UserID, LevelChoice = matchItem.PlayerChoice, PlayerChoice = matchItem.LevelChoice, Result = FlipResult(matchItem.Result)
    }).ToList();

    BotHandler botHandler = new BotHandler();
    IBot? PlayerBot = botHandler.GetBot(playerID);
    if (PlayerBot == null) { return Results.NotFound(); }
    IBot? LevelBot = botHandler.GetBot(levelID);
    if (LevelBot == null) { return Results.NotFound(); }
    
    // Needs to be the wrong way round because each bot thinks they're playing against the player.
    string PlayerChoice = PlayerBot.Play(levelMatches);
    string LevelChoice = LevelBot.Play(playerMatches);

    string playerOutcome = botHandler.DetermineResult(PlayerChoice, LevelChoice);
    Match playerMatch = new Match { PlayerChoice = PlayerChoice, LevelChoice = LevelChoice, Result = playerOutcome, SessionID = request.SessionID, UserID = userID };

    db.MatchItems.Add(playerMatch);
    db.SaveChanges();

    SpectateResponse response = new SpectateResponse { PlayerChoice = PlayerChoice, LevelChoice = LevelChoice, Result = playerOutcome };
    return Results.Ok(response);
  }

  public IResult ValidUser() {
    return Results.Ok();
  }
}