public class Level {
  public int ID { get; set; }
  public string Name { get; set; }
  public ICollection<Session> Sessions { get; set; }
  public ICollection<Match> PlayerMatches { get; set; }
  public ICollection<Match> LevelMatches { get; set; }
}