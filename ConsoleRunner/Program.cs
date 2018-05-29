using Core;
using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRunner
{
  /// <summary>
  /// Console app for running simulation
  /// </summary>
  class Program
  {
    static void Main(string[] args)
    {
      var x = new JsonDataProvider<RoadStats>().GetAll();
      var sim = new Simulation();
      sim.SetUp();
      sim.Start();
      Console.ReadLine();
    }
  }
}
