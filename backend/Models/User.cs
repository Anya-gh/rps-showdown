public class User {
  public int ID { get; set; }
  public required string Username { get; set; }
  public required string Password { get; set; }
  public ICollection<Match> Matches { get; set; } = null!;
  public ICollection<Session> Sessions { get; set; } = null!;
}