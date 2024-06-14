using Microsoft.AspNetCore.SignalR;

public class Session {
  public int ID { get; set; }
  public DateTime StartedAt { get; set; }
  public User User { get; set; }
  public int UserID { get; set; }
  public ICollection<Match> Matches { get; set; }
}