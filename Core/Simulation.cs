#define DEBUG
namespace Core
{
  using Data;
  using Entities;
  using Helpers;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Main simulation class
  /// </summary>
  public partial class Simulation
  {
    /// <summary>
    /// Simulation delta in ms.
    /// More means more accuracy but less speed and vice versa
    /// </summary>
    public const int DELTA_MS = 1000 * 3600 / 10;
    /// <summary>
    /// Number of ms in a day
    /// </summary>
    public const int MS_IN_DAY = 3600 * 1000 * 24;
    protected IDataProvider<TransportStats> TransportStatsProvider { get; set; }
    protected IDataProvider<RoadStats> RoadStatsProvider { get; set; }
    protected Dictionary<string, Road> RoadByFromTo { get; set; } = new Dictionary<string, Road>();
    protected Dictionary<string, Node> NodesByName { get; set; } = new Dictionary<string, Node>();
    protected Dictionary<string, TransportStats> TransportStatsByDirection { get; set; }
      = new Dictionary<string, TransportStats>();
    protected NumberByDaysSpreader Spreader { get; set; }

    /// <summary>
    /// Initialize all variables
    /// </summary>
    public Simulation()
    {
      TransportStatsProvider = new JsonDataProvider<TransportStats>();
      RoadStatsProvider = new JsonDataProvider<RoadStats>();
      Spreader = new NumberByDaysSpreader();
      InitGatherers();
    }

    /// <summary>
    /// Populate dictionaries for later access from dataproviders
    /// </summary>
    public void SetUp()
    {
      PopulateTransportStats();
      PopulateNodes();
      PopulateRoads();
      SetupRoutes();
    }

    /// <summary>
    /// Number of trucks per month based on total
    /// </summary>
    /// <param name="month">Which month</param>
    /// <param name="total">Total number of trucks per year</param>
    /// <returns></returns>
    protected int GetTruckCountForMonth(int month, int total)
    {
      var now = DateTime.Now;
      var days = DateTime.IsLeapYear(now.Year) ? 366 : 365;
      return (int)(DateTime.DaysInMonth(now.Year, month) * 1.0 / days * total);
    }

    /// <summary>
    /// Start the simulation
    /// </summary>
    public void Start()
    {
      var now = DateTime.Now;
      var truckSpreadByDirections = new Dictionary<string, int[]>();
      var trucksByDirections = new Dictionary<string, List<Truck>>();
      var trucks = new List<Truck>();
      long t = 0;

      for (var mon = 1; mon <= 12; mon++)
      {
        Console.WriteLine($"Simulating month {mon}");

        var monthDays = DateTime.DaysInMonth(now.Year, mon);
#if DEBUG
        Console.WriteLine("start randomize");
#endif
        foreach (var (Key, Value) in TransportStatsByDirection)
        {
          var truckCount = GetTruckCountForMonth(mon, Value.PerYear);
          truckSpreadByDirections[Key] = Spreader.Spread(monthDays, truckCount);
        }
#if DEBUG        
        Console.WriteLine("end randomize");
#endif      
        for (var day = 0; day < monthDays; day++)
        {
          foreach (var (Key, Value) in TransportStatsByDirection)
            for (var k = 0; k < truckSpreadByDirections[Key][day]; k++)
            {
              var road = RoadByFromTo[Key];
              trucks.Add(new Truck(road, 70));
            }

          uint dayT = 0;
          while (dayT < MS_IN_DAY)
          {
            PreProcess(trucks);
            trucks.ForEach(truck => truck.Run(t, DELTA_MS));
            PostProcess(trucks);
            ///remove all arrived trucks as they are non necessary
            trucks.RemoveAll(tr => tr.Arrived);
            dayT += DELTA_MS;
            t += DELTA_MS;
          }
        }
      }

      ///Truck count over time to visualize beautiful? chart
      System.IO.File.WriteAllLines("truckCountOverTime.txt",
        TruckCountStatsGatherer.TruckCounts.Select(x => x.ToString()));
      return;
    }

    private void PopulateNodes()
    {
      var roadStats = RoadStatsProvider.GetAll();
      NodesByName = new NodeByNameMaker().Make(roadStats);
    }

    private void PopulateRoads()
    {
      var roadStats = RoadStatsProvider.GetAll();
      RoadByFromTo = new RoadsByDirectionMaker().Make(roadStats, NodesByName);
    }

    private void SetupRoutes()
    {
      var roadStats = RoadStatsProvider.GetAll();
      var transportStats = TransportStatsProvider.GetAll();
      var newRoutes = new RouteMaker().CreateRoutes(roadStats, transportStats);
      foreach (var (Key, Value) in newRoutes)
        RoadByFromTo[Key] = Value;
    }

    private void PopulateTransportStats()
    {
      var transportStats = TransportStatsProvider.GetAll();
      var transportStatsByDirection = new TransportStatsByDirectionMaker().Make(transportStats);
      TransportStatsByDirection = transportStatsByDirection;
    }
  }
}
