namespace Core.Gatherers
{
  using Entities;

  /// <summary>
  /// Updates in,out or transit of node when truck is moving
  /// </summary>
  public class NodeTruckStatistics
  {
    private Truck truck { get; set; }
    private Node node { get; set; }

    /// <summary>
    /// Preinit
    /// </summary>
    /// <param name="truck"></param>
    public void PreProcess(Truck truck) => (this.truck, node) = (truck, GetLastPassedNode());

    /// <summary>
    /// Get last node truck has passed
    /// </summary>
    /// <returns></returns>
    private Node GetLastPassedNode() => truck.Road.GetCurrentRoad(truck.DrivenKms).From;

    /// <summary>
    /// Post process
    /// </summary>
    public void PostProcess()
    {
      var lastNode = GetLastPassedNode();
      if (lastNode == node)
        return;

      var trackRoad = truck.Road;
      if (trackRoad.To == lastNode && truck.Arrived)
        lastNode.In++;
      else if (trackRoad.From == lastNode && truck.DrivenKms == 0)
        lastNode.Out++;
      else if (trackRoad.To != lastNode && trackRoad.From != lastNode)
        lastNode.Transit++;
    }
  }
}
