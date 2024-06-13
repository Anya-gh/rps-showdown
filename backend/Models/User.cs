public class User {
  public int ID { get; set; }
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public required List<Record> Records { get; set; }
  public required UserStats UserStats { get; set; }
}