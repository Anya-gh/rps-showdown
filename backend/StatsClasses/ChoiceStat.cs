public class ChoiceStat {

  // change to class of choice and value where value is a float and change from TimesUsed to ChoiceStats
  public string Choice { get; set; }
  public float Stat { get; set; }
  public ChoiceStat(string choice, float stat) {
    Choice = choice;
    Stat = stat;
  }
}