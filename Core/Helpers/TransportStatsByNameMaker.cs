namespace Core.Helpers
{
  using System.Collections.Generic;
  using Data;

  /// <summary>
  /// Creates dictionary that accesses transport stats by direction
  /// </summary>
  public class TransportStatsByDirectionMaker
  {
    public Dictionary<string, TransportStats> Make(List<TransportStats> transportStats)
    {
      var transportStatsByDir = new Dictionary<string, TransportStats>();

      foreach (var transportStat in transportStats)
        transportStatsByDir[transportStat.ToString()] = transportStat;

      return transportStatsByDir;
    }
  }
}
