public class Record {
  public int ID { get; set; }
  public string Level { get; set; } = "";
  public string PlayerChoice { get; set; } = "";
  public string AIChoice { get; set; } = "";
  public string Result { get; set; } = "";
  public User User { get; set; }
  public int UserID { get; set; }
}