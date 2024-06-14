using System.Xml.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;

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
      List<int> levelIDs = db.LevelItems.Select(levelItem => levelItem.ID).ToList();
      List<UserStats> newUserStats = levelIDs.Select(levelID => 
        new UserStats { Ace = "None", Nemesis = "None", Wins = 0, Draws = 0, Losses = 0, LongestStreak = 0, TimesRockUsed = 0, TimesPaperUsed = 0, TimesScissorsUsed = 0, PlayStyle = "None", LeveLID = levelID }
      ).ToList();
      User newUser = new User { Username = user.Username, Password = user.Password, UserStats = newUserStats };
      db.UserItems.Add(newUser);
      db.SaveChanges();
      return Results.Ok(SecurityHandler.CreateToken(user));
    }
  }

  public IResult Stats(UserDetails user, RPSDbContext db) {

    (float, float, float) ChoiceDistribution(int rock, int paper, int scissors) {
      var total = Math.Max(rock + paper + scissors, 1);
      return (rock/total, paper/total, scissors/total);
    }

    int? userID = (
      from userItem in db.UserItems
      where userItem.Username == user.Username
      select userItem.ID
    ).FirstOrDefault();

    var query = (
      from userStatsItem in db.UserStatsItems
      join levelItem in db.LevelItems on userStatsItem.LeveLID equals levelItem.ID
      where userStatsItem.UserID == userID
      select new UserStatsJoinLevel(userStatsItem, levelItem)
    ).ToList();

    if (query != null && query.Count() == 3) {
      List<UserStatsInfo> userStatsInfo = new List<UserStatsInfo>();

      foreach (var userStats in query) {
        UserStats stats = userStats.UserStats;
        var winRate = stats.Wins / Math.Max(stats.Wins + stats.Losses, 1);
        var choiceDistribution = ChoiceDistribution(stats.TimesRockUsed, stats.TimesPaperUsed, stats.TimesScissorsUsed);
        userStatsInfo.Add(new UserStatsInfo(winRate, stats.LongestStreak, choiceDistribution, stats.Ace, stats.Nemesis, stats.PlayStyle, stats.Level.Name));
      }

      return Results.Ok(userStatsInfo);
    }
    else {
      Console.WriteLine("Query is null.");
      return Results.NotFound();
    }
  }

  public IResult CreateSession(UserDetails user, RPSDbContext db) {
    int userID = (
      from userItem in db.UserItems
      where userItem.Username == user.Username
      select userItem.ID
    ).FirstOrDefault();
    if (userID < 1) { return Results.NotFound(); }
    DateTime startedAt = DateTime.UtcNow;
    Session newSession = new Session { UserID = userID, StartedAt = startedAt };
    try { 
      db.SessionItems.Add(newSession);
      db.SaveChanges();
      int sessionID = db.SessionItems.Where(session => session.UserID == userID && session.StartedAt == startedAt).Select(session => session.ID).FirstOrDefault();
      if (sessionID < 1) { return Results.StatusCode(500); }
      return Results.Ok(sessionID);
    }
    catch (Exception ex) {
      Console.WriteLine(ex.Message);
      return Results.StatusCode(500);
    }

  }

  public IResult ValidUser() {
    return Results.Ok();
  }
}