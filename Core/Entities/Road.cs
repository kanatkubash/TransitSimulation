namespace Core.Entities
{
  using Helpers;
  using System.Linq;
  using Newtonsoft.Json;

  /// <summary>
  /// Class for roads, routes
  /// </summary>
  public class Road : SimItem
  {
    public string Name { get; }
    public double Length
    {
      get => Roads?.Sum(r => r.Value.Length) ?? length;
      private set => length = value;
    }
    private double length = double.NaN;
    /// <summary>
    /// Subroads
    /// </summary>
    public RangeMap<Road> Roads { get; }
    public Node From { get; set; }
    public Node To { get; set; }

    [JsonConstructor()]
    public Road(string name, double length) => (Name, Length) = (name, length);

    public Road(string name, RangeMap<Road> roads) => (Name, Roads) = (name, roads);

    /// <summary>
    /// Get subroad by km
    /// </summary>
    /// <param name="km">Km of road</param>
    /// <returns>Road that corresponds to given km</returns>
    public Road GetCurrentRoad(double km) => Roads?[(int)km]?.GetCurrentRoad(km) ?? this;

    public override string ToString() => $"{From}-{To}";

    /// <summary>
    /// Creates road that is reverse of this road
    /// </summary>
    /// <returns>New road that has from ,to reversed</returns>
    public Road Reverse()
    {
      RangeMap<Road> roads = null;
      if (Roads != null)
      {
        roads = new RangeMap<Road>(Roads.MinValue, Roads.MaxValue);
        var roadValues = Roads.Values.Reverse().ToArray();
        for (var i = 0; i < roadValues.Length; i++)
          roads[i == 0 ? 0 : (int)roadValues[i - 1].Length] = roadValues[i];
      }

      return roads == null
        ? new Road(Name, Length)
        {
          From = To,
          To = From
        }
        : new Road(Name, roads)
        {
          From = To,
          To = From
        };
    }

    public string ComputedName => From.Name.CompareTo(To.Name) > 0 ? $"{From}-{To}" : $"{To}-{From}";
  }
}
