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

    public override string ToString() => $"{From}-{To}";
  }
}
