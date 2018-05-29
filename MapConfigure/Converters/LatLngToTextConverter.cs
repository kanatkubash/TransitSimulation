namespace MapConfigure.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  using GMap.NET;

  class LatLngToTextConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var latLng = (PointLatLng)value;
      return Math.Round(latLng.Lat, 4) + "  " + Math.Round(latLng.Lng, 4);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => throw new NotImplementedException();
  }
}
