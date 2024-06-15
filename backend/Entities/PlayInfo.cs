public class PlayInfo {
  public int LevelID { get; set; }
  public string Name { get; set; }

  public PlayInfo(int levelID, string name) {
    LevelID = levelID;
    Name = name;
  }
}