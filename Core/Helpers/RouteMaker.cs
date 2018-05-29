namespace Core.Helpers
{
  using Data;
  using Entities;
  using Dijkstra.NET.Contract;
  using Dijkstra.NET.Model;
  using Dijkstra.NET.ShortestPath;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Finds least distant routes from node A to B that are given as transportStat array
  /// No need to use if road information is given already
  /// </summary>
  public class RouteMaker
  {
    protected Dictionary<string, Road> RoadByFromTo { get; set; } = new Dictionary<string, Road>();
    protected Dictionary<string, Node> NodesByName { get; set; } = new Dictionary<string, Node>();

    public Dictionary<string, Road> CreateRoutes(List<RoadStats> roadList, List<TransportStats> transportStats)
    {
      Setup(roadList);

      var newRoadsByDirection = new Dictionary<string, Road>();
      var graph = new Graph<Node, string>();
      var nodeIndexByNode = new Dictionary<Node, uint>();
      uint counter = 0;

      foreach (var (Key, Value) in NodesByName)
      {
        nodeIndexByNode[Value] = counter++;
        graph.AddNode(Value);
      }

      uint GetIOfNode(Node n) => nodeIndexByNode[n];
      Node GetNodeByName(string name) => NodesByName[name];

      foreach (var (Key, Value) in RoadByFromTo)
        graph.Connect(GetIOfNode(Value.From), GetIOfNode(Value.To), (int)Value.Length, Key);

      var solver = new Dijkstra<Node, string>(graph);
      foreach (var transportStat in transportStats)
      {
        if (RoadByFromTo.ContainsKey(transportStat.ToString()))
          continue;
        var result = solver.Process(
          GetIOfNode(GetNodeByName(transportStat.From)),
          GetIOfNode(GetNodeByName(transportStat.To))
          );

        var newRoute = MakeCompositeRoad(graph, result, transportStat);
        newRoadsByDirection[transportStat.ToString()] = newRoute;
      }

      return newRoadsByDirection;
    }

    protected void Setup(List<RoadStats> roadList)
    {
      NodesByName = new NodeByNameMaker().Make(roadList);
      RoadByFromTo = new RoadsByDirectionMaker().Make(roadList, NodesByName);
    }

    protected Road MakeCompositeRoad(Graph<Node, string> graph, IShortestPathResult result, TransportStats transportStat)
    {
      var roads = new RangeMap<Road>(0, result.Distance);
      Node GetNodeByIndex(uint index) => graph[index].Item;

      var optimalPaths = result.GetPath().ToArray();
      if (optimalPaths.Length <= 1)
        throw new Exception($"Route for {transportStat}  not found");
      roads[0] =
        RoadByFromTo[
          transportStat.From
          + "-"
          + GetNodeByIndex(optimalPaths[1]).Name
          ];
      var prev = roads[0];
      for (var i = 1; i < optimalPaths.Length; i++)
        roads[(int)prev.Length] = RoadByFromTo[
          GetNodeByIndex(optimalPaths[i - 1]).Name
          + "-"
          + GetNodeByIndex(optimalPaths[i]).Name
        ];

      return new Road(transportStat.Name, roads)
      {
        From = NodesByName[transportStat.From],
        To = NodesByName[transportStat.To],
      };
    }
  }
}
