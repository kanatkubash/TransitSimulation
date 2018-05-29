using Core;
using Core.Data;
using Core.Entities;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreTest
{
  public class MoqSim : Simulation
  {
    public MoqSim()
    {
      var mockTransportStat = new Mock<IDataProvider<TransportStats>>();
      mockTransportStat.Setup(s => s.GetAll()).Returns(new List<TransportStats>
      {
        new TransportStats(){ From="1",To="3",PerYear=100},
        new TransportStats(){ From="1",To="2",PerYear=50},
      });
      this.TransportStatsProvider = mockTransportStat.Object;

      var mockRoadStats = new Mock<IDataProvider<RoadStats>>();
      mockRoadStats.Setup(m => m.GetAll()).Returns(new List<RoadStats>()
      {
        new RoadStats(){ From="1",To="2", Length=15},
        new RoadStats(){ From="2",To="3", Length=10},
        new RoadStats(){ From="1",To="4", Length=15},
        new RoadStats(){ From="4",To="3", Length=15},
      });
      this.RoadStatsProvider = mockRoadStats.Object;
    }

    public Dictionary<string, Road> GetRoadMap() => RoadByFromTo;
  }
  [TestFixture]
  public class SimulationTest
  {
    [Test]
    public void TestRouteFind()
    {
      var sim = new MoqSim();
      sim.SetUp();
      var roads = sim.GetRoadMap();
      Assert.AreEqual(roads["1-3"].Length, 25);
      Assert.AreEqual(roads["1-3"].Roads[0].Length, 15);
      Assert.AreEqual(roads["1-3"].Roads[14].Length, 15);
      Assert.AreEqual(roads["1-3"].Roads[15].Length, 10);
    }
  }
}
