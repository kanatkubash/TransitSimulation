using Core.Entities;
using Core.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTest
{
  [TestFixture]
  public class RoadTest
  {
    [Test]
    public void TestRoadReverse()
    {
      var road = new Road("1", 11);
      var revRoad = road.Reverse();
      Assert.IsNull(revRoad.Roads);
      road = new Road("x", new RangeMap<Road>(0, 1000)
      {
        [0] = new Road("x", 1),
        [1] = new Road("y", 2),
      });
      revRoad = road.Reverse();
      Assert.AreEqual(revRoad.Length, 3);
      Assert.AreEqual(revRoad.Roads[0].Length, 2);
      Assert.AreEqual(revRoad.Roads[2].Length, 1);
      road = new Road("x", new RangeMap<Road>(0, 1000)
      {
        [0] = new Road("x", 3),
        [3] = new Road("y", 5),
      });
      revRoad = road.Reverse();
      Assert.AreEqual(revRoad.Length, 8);
      Assert.AreEqual(revRoad.Roads[0].Length, 5);
      Assert.AreEqual(revRoad.Roads[5].Length, 3);
    }
  }
}
