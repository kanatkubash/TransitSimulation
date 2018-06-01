namespace MapConfigure.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  using Core.Data;

  public class TrStatsToRouteConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      TransportStats tStats = (TransportStats)value;
      var from = tStats.From;
      var to = tStats.To;
      return from.CompareTo(to) > 0 ? $"{from}-{to}" : $"{to}-{from}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => throw new NotImplementedException();
  }
}
