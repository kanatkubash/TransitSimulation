namespace MapConfigure.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  public class Invertor : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      => !((bool)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => throw new NotImplementedException();
  }
}
