namespace Core.Data
{
  /// <summary>
  /// Keeps road data such as km between from to cities
  /// </summary>
  public class RoadStats
  {
    public string From { get; set; }
    public string To { get; set; }
    public int Length { get; set; }

    public RoadStats Reverse() => new RoadStats()
    {
      From = To,
      To = From,
      Length = Length,
    };

    public override string ToString() => $"{From}-{To}";
  }
}
