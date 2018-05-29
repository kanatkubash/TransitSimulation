namespace Core.Gatherers
{
  using Entities;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Counts maximum amount of trucks that are on the road
  /// </summary>
  public class TruckCountStatistics
  {
    /// <summary>
    /// Number of trucks on road over time
    /// </summary>
    public List<int> TruckCounts { get; private set; } = new List<int>();
    private List<Truck> Trucks { get; set; }
    /// <summary>
    /// Maximum peak number of trucks that ever registered
    /// </summary>
    public int Max
    {
      get => max;
      set => max = Math.Max(value, max);
    }
    private int max = 0;

    public void PreProcess(List<Truck> trucks) => Trucks = trucks;

    public void PostProcess()
    {
      TruckCounts.Add(Trucks.Count(t => !t.Arrived));
      Max = Trucks.Count(t => !t.Arrived);
    }
  }
}
