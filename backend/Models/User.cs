public class User {
  public int ID { get; set; }
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public ICollection<Match> Matches { get; set; }
  public ICollection<UserStats> UserStats { get; set; }
  public ICollection<Session> Sessions { get; set; }
}