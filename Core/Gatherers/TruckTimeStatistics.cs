namespace Core.Gatherers
{
  using Entities;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Counts truck arrival times in min,max
  /// </summary>
  public class TruckTimeStatistics
  {
    public int ArrivedCount { get; private set; }
    public long TotalPassedSeconds { get; private set; }
    public int Max
    {
      get => max;
      set => max = Math.Max(value, max);
    }
    private int max = 0;
    public int Min
    {
      get => min;
      set => min = Math.Min(value, min);
    }
    private int min = 0;
    public long Average => ArrivedCount == 0 ? 0 : TotalPassedSeconds / ArrivedCount;

    public void PreProcess() { }

    public void PostProcess(List<Truck> trucks)
    {
      var arriveds = trucks.Where(t => t.Arrived);
      ArrivedCount += arriveds.Count();
      if (arriveds.Count() == 0)
        return;
      Max = (int)arriveds.Max(t => t.PassedSeconds);
      Min = (int)arriveds.Min(t => t.PassedSeconds);
      TotalPassedSeconds += (long)Math.Round(arriveds.Sum(t => t.PassedSeconds));
    }
  }
}
