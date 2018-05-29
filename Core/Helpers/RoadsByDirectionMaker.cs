using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
  /// <summary>
  /// Creates dictionary where roads direction is used as key. Reverse road is also added
  /// </summary>
  public class RoadsByDirectionMaker
  {
    public Dictionary<string, Road> Make(List<RoadStats> roadStats, Dictionary<string, Node> nodesByName)
    {
      var roadsByDirection = new Dictionary<string, Road>();

      foreach (var road in roadStats)
      {
        var roadItem = new Road(road.ToString(), road.Length)
        {
          From = nodesByName[road.From],
          To = nodesByName[road.To],
        };
        roadsByDirection[road.ToString()] = roadItem;
        roadsByDirection[road.Reverse().ToString()] = roadItem.Reverse();
      }

      return roadsByDirection;
    }
  }
}
