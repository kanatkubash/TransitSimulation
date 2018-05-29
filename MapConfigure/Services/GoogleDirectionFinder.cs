namespace MapConfigure.Services
{
  using Core.Entities;
  using GMap.NET;
  using GMap.NET.MapProviders;

  public class GoogleDirectionFinder
  {
    public GDirections GetDirection(Node a, Node b)
    {
      GoogleMapProvider.Instance.GetDirections(out GDirections gDirections, a.LatLng, b.LatLng,
        false, false, false, false, false);
      return gDirections;
    }
  }
}
