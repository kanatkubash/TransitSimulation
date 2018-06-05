using System.Linq;
using Core.Helpers;

namespace Core
{
  using System;
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
    public SealCountStatistics SealCountStatsGatherer { get; protected set; }

    /// <summary>
    /// Set up some variables right before approaching next iteration
    /// </summary>
    /// <param name="trucks"></param>
    /// <param name="time">Simulation time</param>
    protected void PreProcess(List<Truck> trucks, long time)
    {
      //this.NodeTruckStatsGatherer.PreProcess()
      TruckCountStatsGatherer.PreProcess(trucks);
      TruckTimeStatsGatherer.PreProcess();
      SealCountStatsGatherer.PreProcess(trucks, time);
    }

    /// <summary>
    /// Update some variables right after simulation iteration is run
    /// </summary>
    /// <param name="trucks"></param>
    /// <param name="time">Simulation time</param>
    protected void PostProcess(List<Truck> trucks, long time)
    {
      TruckCountStatsGatherer.PostProcess();
      TruckTimeStatsGatherer.PostProcess(trucks);
      SealCountStatsGatherer.PostProcess(trucks, time);
    }

    /// <summary>
    /// Initial setup of gatherers
    /// </summary>
    protected void InitGatherers()
    {
      //this.NodeTruckStatsGatherer = new NodeTruckStatistics();
      TruckCountStatsGatherer = new TruckCountStatistics();
      TruckTimeStatsGatherer = new TruckTimeStatistics();
      SealCountStatsGatherer = new SealCountStatistics(
        new NodeByNameMaker().Make(RoadStatsProvider.GetAll()).Keys,
        DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365);
    }
  }
}
