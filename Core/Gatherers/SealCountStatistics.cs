namespace Core.Gatherers
{
  using Entities;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Data;

  public class SealCountStatistics
  {
    public IEnumerable<string> NodeNames { get; }
    public ObservableCollection<SealStats> SealStats { get; protected set; }
    public int[] SealDistributions { get; protected set; } = { 0, 0, 0, 0 };
    private readonly int days;

    public SealCountStatistics(IEnumerable<string> nodes, int days)
    {
      NodeNames = nodes;
      this.days = days;
      SealStats = new ObservableCollection<SealStats>();
      foreach (var node in NodeNames)
        SealStats.Add(new SealStats()
        {
          Node = node,
          Increments = new int[days],
          Inquiries = new int[days],
          Reuses = new int[days],
          Stock = new int[days],
        });
    }

    public void PreProcess(List<Truck> trucks, long time)
    {
      var dayIndex = GetDayIndexFromMs(time);

      var newTrucksGroup = trucks.Where(t => t.DrivenKms == 0).GroupBy(t => t.Road.From.Name);
      foreach (var newTruckList in newTrucksGroup)
      {
        var stat = SealStats.First(s => s.Node == newTruckList.Key);
        newTruckList.ForEach(t => SealDistributions[t.Seals - 1]++);
        var inquirySeals = newTruckList.Sum(t => t.Seals);
        var prevStock = dayIndex == 0 ? 000 : stat.Stock[dayIndex - 1];
        var prevReuse = dayIndex == 0 ? 000 : stat.Reuses[dayIndex - 1];
        stat.Inquiries[dayIndex] += inquirySeals;
        if (prevStock < inquirySeals)
        {
          var delta = 0;
          if (prevStock <= 0)
            delta = inquirySeals - prevReuse;
          else
            delta = inquirySeals - prevStock - prevReuse;
          stat.Increments[dayIndex] = Math.Abs(delta);
          stat.Minimum += Math.Abs(delta);
        }

        stat.Stock[dayIndex] = prevStock - inquirySeals;
      }
    }

    public void PostProcess(List<Truck> trucks, long time)
    {
      var dayIndex = GetDayIndexFromMs(time);

      #region reuses
      var arrivedTrucksGroup = trucks.Where(t => t.Arrived).GroupBy(t => t.Road.To.Name);
      foreach (var arrivedTruckList in arrivedTrucksGroup)
      {
        var stat = SealStats.First(s => s.Node == arrivedTruckList.Key);
        var prevStock = dayIndex == 0 ? 000 : stat.Stock[dayIndex - 1];
        var reuseSeals = arrivedTruckList.Sum(t => t.Seals);
        stat.Reuses[dayIndex] += reuseSeals;
        stat.Stock[dayIndex] = prevStock + stat.Reuses[dayIndex] - stat.Inquiries[dayIndex];
      }
      #endregion
    }

    private int GetDayIndexFromMs(long ms)
    {
      var day = ms / Simulation.MS_IN_DAY;
      //if (ms % Simulation.MS_IN_DAY == 0) day--;
      //if (day < 0) day = 0;
      //else if (day >= days) day--;
      return (int)day;
    }
  }
}
