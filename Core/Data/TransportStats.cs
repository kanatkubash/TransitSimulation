namespace Core.Data
{
  /// <summary>
  /// Represents truck statistics by route
  /// </summary>
  public class TransportStats
  {
    public string From { get; set; }
    public string To { get; set; }
    public int PerYear { get; set; }
    public string Name { get; set; }

    public TransportStats Reverse() => new TransportStats()
    {
      From = To,
      To = From,
      PerYear = 0,
      Name = Name
    };

    public string ComputedName => From.CompareTo(To) > 0 ? $"{From}-{To}" : $"{To}-{From}";

    public override string ToString() => $"{From}-{To}";
  }
}
