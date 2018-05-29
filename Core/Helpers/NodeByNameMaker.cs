namespace Core.Helpers
{
  using Data;
  using Entities;
  using System.Collections.Generic;

  /// <summary>
  /// Creates dictionary of nodes accessed by node name
  /// </summary>
  public class NodeByNameMaker
  {
    public Dictionary<string, Node> Make(List<RoadStats> roads)
    {
      var nodesByName = new Dictionary<string, Node>();

      foreach (var road in roads)
      {
        if (!nodesByName.ContainsKey(road.From))
          nodesByName[road.From] = new Node(road.From);
        if (!nodesByName.ContainsKey(road.To))
          nodesByName[road.To] = new Node(road.To);
      }

      return nodesByName;
    }
  }
}
