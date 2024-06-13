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
        return Results.Ok(SecurityHandler.CreateToken(user));
      }
      else { 
        return Results.Unauthorized();
      }
    }
    else {
      // create user
      UserStats newUserStats = new UserStats { };
      User newUser = new User { Username = user.Username, Password = user.Password, UserStats = newUserStats };
      db.UserItems.Add(newUser);
      db.SaveChanges();
      return Results.Ok(SecurityHandler.CreateToken(user));
    }
  }
}