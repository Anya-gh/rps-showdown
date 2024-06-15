public class PlayResponse {
  public string BotChoice { get; set; }
  public string Result { get; set; }

  public PlayResponse(string botChoice, string result) {
    BotChoice = botChoice;
    Result = result;
  }
}