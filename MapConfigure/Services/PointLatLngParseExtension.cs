namespace MapConfigure.Services
{
  using System;
  using GMap.NET;
  using Newtonsoft.Json.Linq;

  public class LatLngParser
  {
    public static PointLatLng Parse(object obj)
    {
      if (obj is PointLatLng)
        return (PointLatLng)obj;
      var json = (JObject)obj;
      return new PointLatLng(
        Convert.ToDouble(json["Lat"]),
        Convert.ToDouble(json["Lng"])
      );
    }
  }
}
