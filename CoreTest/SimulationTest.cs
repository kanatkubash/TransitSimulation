using Core;
using Core.Data;
using Core.Entities;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Helpers;

namespace CoreTest
{
  public class MoqSpreader : NumberByDaysSpreader
  {
    protected override IRandom Random { get; }

    public static NumberByDaysSpreader Create()
    {
      int call1 = 0, call2 = 0;
      var moqRandom = new Mock<NumberByDaysSpreader>();
      moqRandom
        .Setup(m => m.Spread(
          It.IsAny<int>(),
          It.IsAny<int>()))
        .Returns(new Func<int, int, int[]>((a, b) => new int[a]));
      moqRandom
        .Setup(m =>
        m.Spread(
          It.Is<int>(a => a == 31),
          It.Is<int>(a => a == 84)
          ))
        .Returns(() =>
        {
          if (call1++ > 0)
            return new int[31];
          var a = new List<int>();
          a.AddRange(new int[] { 50, 150, 100, 150, 150, 100, 100 });
          a.AddRange(new int[31 - 7]);
          return a.ToArray();
        });
      moqRandom
        .Setup(m =>
          m.Spread(
            It.Is<int>(a => a == 31),
            It.Is<int>(a => a == 1)
          ))
        .Returns(() =>
        {
          if (call2++ > 0)
            return new int[31];
          var a = new List<int>();
          a.AddRange(new int[] { 0, 0, 0, 0, 0, 10 });
          a.AddRange(new int[31 - 6]);
          return a.ToArray();
        });
      return moqRandom.Object;
    }
  }

  public class MoqSealCountSpreader : SealCountSpreader
  {
    public override int[] Spread(int count, int[] distributions)
    {
      return Enumerable.Repeat(2, 10000).ToArray();
    }
  }
  public class MoqSim : Simulation
  {
    public static MoqSim Create()
    {
      var mockTransportStat = new Mock<IDataProvider<TransportStats>>();
      mockTransportStat.Setup(s => s.GetAll()).Returns(new List<TransportStats>
      {
        new TransportStats(){ From="1",To="3",PerYear=100},
        new TransportStats(){ From="1",To="2",PerYear=50},
      });
      var tsProvider = mockTransportStat.Object;

      var mockRoadStats = new Mock<IDataProvider<RoadStats>>();
      mockRoadStats.Setup(m => m.GetAll()).Returns(new List<RoadStats>()
      {
        new RoadStats(){ From="1",To="2", Length=15},
        new RoadStats(){ From="2",To="3", Length=10},
        new RoadStats(){ From="1",To="4", Length=15},
        new RoadStats(){ From="4",To="3", Length=15},
      });
      var rsProvider = mockRoadStats.Object;
      return new MoqSim(tsProvider, rsProvider);
    }

    public static MoqSim CreateToCheckSealDistribution()
    {
      var mockTransportStat = new Mock<IDataProvider<TransportStats>>();
      mockTransportStat.Setup(s => s.GetAll()).Returns(new List<TransportStats>
      {
        new TransportStats(){ From="1",To="2",PerYear=1000},
        new TransportStats(){ From="2",To="1",PerYear=20},
      });
      var tsProvider = mockTransportStat.Object;

      var mockRoadStats = new Mock<IDataProvider<RoadStats>>();
      mockRoadStats.Setup(m => m.GetAll()).Returns(new List<RoadStats>()
      {
        new RoadStats(){ From="1",To="2", Length=400},
      });
      var rsProvider = mockRoadStats.Object;
      return new MoqSim(tsProvider, rsProvider);
    }

    public static MoqSim CreateToCheckSealStats()
    {
      var mockTransportStat = new Mock<IDataProvider<TransportStats>>();
      mockTransportStat.Setup(s => s.GetAll()).Returns(new List<TransportStats>
      {
        new TransportStats(){ From="1",To="2",PerYear=1000},
        new TransportStats(){ From="2",To="1",PerYear=20},
      });
      var tsProvider = mockTransportStat.Object;

      var mockRoadStats = new Mock<IDataProvider<RoadStats>>();
      mockRoadStats.Setup(m => m.GetAll()).Returns(new List<RoadStats>()
      {
        new RoadStats(){ From="1",To="2", Length=400},
      });
      var rsProvider = mockRoadStats.Object;
      return new MoqSim(tsProvider, rsProvider)
      {
        Spreader = MoqSpreader.Create(),
        SealCountSpreader = new MoqSealCountSpreader(),
      };
    }

    protected MoqSim(IDataProvider<TransportStats> tsProvider, IDataProvider<RoadStats> rsProvider) : base(tsProvider,
      rsProvider, new EqualDistribution())
    {
    }

    public Dictionary<string, Road> GetRoadMap() => RoadByFromTo;
  }
  [TestFixture]
  public class SimulationTest
  {
    [Test]
    public void TestRouteFind()
    {
      var sim = MoqSim.Create();
      sim.SetUp();
      var roads = sim.GetRoadMap();
      Assert.AreEqual(roads["1-3"].Length, 25);
      Assert.AreEqual(roads["1-3"].Roads[0].Length, 15);
      Assert.AreEqual(roads["1-3"].Roads[14].Length, 15);
      Assert.AreEqual(roads["1-3"].Roads[15].Length, 10);
    }

    /// <summary>
    /// Check if seal stats such as stock,inquiry,increment,minimum are correct
    /// </summary>
    [Test]
    public void TestSealStats()
    {
      var sim = MoqSim.CreateToCheckSealStats();
      sim.SetUp();
      sim.Start(70, new int[] { 25, 25, 30, 20 }, default(CancellationToken));
      var sealStat1 = sim.SealCountStatsGatherer.SealStats.First(s => s.Node == "1");
      var sealStat2 = sim.SealCountStatsGatherer.SealStats.First(s => s.Node == "2");
      CollectionAssert.AreEqual(
        sealStat1.Stock,
        new List<int>(365).Init(new[] { -100, -400, -600, -900, -1200, -1380, -1580 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat1.Inquiries,
        new List<int>(365).Init(new[] { 100, 300, 200, 300, 300, 200, 200 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat1.Increments,
        new List<int>(365).Init(new[] { 100, 300, 200, 300, 300, 200, 180 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat1.Reuses,
        new List<int>(365).Init(new[] { 0, 0, 0, 0, 0, 20, 0 }).ToArray());
      Assert.AreEqual(sealStat1.Minimum, 1580);

      CollectionAssert.AreEqual(
        sealStat2.Stock,
        new List<int>(365).Init(new[] { 100, 400, 600, 900, 1200, 1380, 1580 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat2.Inquiries,
        new List<int>(365).Init(new[] { 0, 0, 0, 0, 0, 20, 0 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat2.Increments,
        new List<int>(365).Init(new[] { 0, 0, 0, 0, 0, 0, 0 }).ToArray());
      CollectionAssert.AreEqual(
        sealStat2.Reuses,
        new List<int>(365).Init(new[] { 100, 300, 200, 300, 300, 200, 200 }).ToArray());
      Assert.AreEqual(sealStat2.Minimum, 0);
    }

    /// <summary>
    /// Check seal count distribution among trucks is kept
    /// </summary>
    [Test]
    public void TestSealCountDistribution()
    {
      var sim = MoqSim.CreateToCheckSealDistribution();
      sim.SetUp();
      sim.Start(30, new int[] { 25, 25, 30, 20 }, default(CancellationToken));
      var sealInfo = sim.SealCountStatsGatherer;
      var totalSeals = sealInfo.SealStats.Sum(s => s.Inquiries.Sum());
      Assert.AreEqual(totalSeals,
        sealInfo.SealDistributions[0] * 1
        + sealInfo.SealDistributions[1] * 2
        + sealInfo.SealDistributions[2] * 3
        + sealInfo.SealDistributions[3] * 4);
    }
  }

  public static class ListInitExtension
  {
    public static List<T> Init<T>(this List<T> list, T[] items)
    {
      list.AddRange(items);
      list.AddRange(new T[list.Capacity - items.Length]);
      return list;
    }
  }
}
