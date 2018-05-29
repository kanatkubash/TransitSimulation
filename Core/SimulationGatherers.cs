namespace Core
{
  using Entities;
  using Gatherers;
  using System.Collections.Generic;

  /// <summary>
  /// Part of simulation class with gatherers
  /// </summary>
  public partial class Simulation
  {
    //public NodeTruckStatistics NodeTruckStatsGatherer { get;protected set; }
    public TruckCountStatistics TruckCountStatsGatherer { get; protected set; }
    public TruckTimeStatistics TruckTimeStatsGatherer { get; protected set; }

    /// <summary>
    /// Set up some variables right before approaching next iteration
    /// </summary>
    /// <param name="trucks"></param>
    protected void PreProcess(List<Truck> trucks)
    {
      //this.NodeTruckStatsGatherer.PreProcess()
      TruckCountStatsGatherer.PreProcess(trucks);
      TruckTimeStatsGatherer.PreProcess();
    }

    /// <summary>
    /// Update some variables right after simulation iteration is run
    /// </summary>
    /// <param name="trucks"></param>
    protected void PostProcess(List<Truck> trucks)
    {
      TruckCountStatsGatherer.PostProcess();
      TruckTimeStatsGatherer.PostProcess(trucks);
    }

    /// <summary>
    /// Initial setup of gatherers
    /// </summary>
    protected void InitGatherers()
    {
      //this.NodeTruckStatsGatherer = new NodeTruckStatistics();
      TruckCountStatsGatherer = new TruckCountStatistics();
      TruckTimeStatsGatherer = new TruckTimeStatistics();
    }
  }
}
