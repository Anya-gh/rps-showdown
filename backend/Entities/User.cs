public class User {
  public int ID { get; set; }
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public ICollection<Record> Records { get; set; }
  public UserStats UserStats { get; set; }
}