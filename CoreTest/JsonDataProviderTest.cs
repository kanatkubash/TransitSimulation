using Core.Data;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;

namespace CoreTest
{
  [TestFixture]
  public class JsonDataProviderTest
  {
    [Test]
    public void TestFileCreation()
    {
      var dir = Directory.GetCurrentDirectory();
      var jsonFile = $@"{dir}\TransportStats.json";
      File.Delete(jsonFile);
      var jsonProvider = new JsonDataProvider<TransportStats>();
      var data = jsonProvider.GetAll();
      Assert.That(File.Exists(jsonFile));
      return;
    }

    [Test]
    public void TestReadWrite()
    {
      var dir = Directory.GetCurrentDirectory();
      var jsonFile = $@"{dir}\TransportStats.json";
      var jsonProvider = new JsonDataProvider<TransportStats>();
      File.Delete(jsonFile);
      var data = jsonProvider.GetAll();
      Assert.AreEqual(0, data.Count);
      jsonProvider.SaveAll(new List<TransportStats>(
        new[] {
          new TransportStats()
          {
             From="ala",
             To="guw",
             PerYear=1000
          },
          new TransportStats()
          {
             From="guw",
             To="ala",
             PerYear=100
          },
        }));
      data = jsonProvider.GetAll();
      Assert.AreEqual(data.Count, 2);
      Assert.AreEqual(data[0].From, "ala");
      Assert.AreEqual(data[0].PerYear, 1000);
      Assert.AreEqual(data[1].To, "ala");
      Assert.AreEqual(data[1].PerYear, 100);
    }
  }
}
