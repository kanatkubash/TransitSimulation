namespace MapConfigure.Components
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Windows.Controls;
  using System.Windows.Media;
  using System.Windows.Shapes;
  using Core.Entities;
  using GMap.NET;
  using GMap.NET.WindowsPresentation;
  using Services;
  using ViewModels;

  public class CRoute : GMapRoute
  {
    public RoadsViewModel RoadsVm { get; }
    public Road Road { get; }
    public RoadViewModel RoadVm { get; }
    public object Brush
    {
      get => brush = brush ?? Brushes.Black;
      private set
      {
        if (value == brushText)
          return;
        brushText = value;
        brush = value is SolidColorBrush ? value : new BrushConverter().ConvertFrom(value);
      }
    }
    private object brush;
    private object brushText;
    private ContextMenu contextMenu;

    protected CRoute(IEnumerable<PointLatLng> points, RoadsViewModel roadVm, Road road)
      : base(points)
    {
      (RoadsVm, Road) = (roadVm, road);
      RoadVm = new RoadViewModel(RoadsVm, road);
      contextMenu = new ContextMenu()
      {
        Items =
        {
          new MenuItem()
          {
            Header = "Удалить",
            Command = RoadVm.RemoveRoadCommand,
            CommandParameter = Road,
          }
        },
      };

      //var colorHex = Road["Color"];
      //ColorVm = new ColorViewModel()
      //{
      //  Color = (Color)ColorConverter.ConvertFromString((string)(Road["Color"] ?? "#000")),
      //  ColorText = (string)Road["Color"],
      //};
      //ColorVm.PropertyChanged += ColorVm_PropertyChanged;
    }

    //private void ColorVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //  Debug.WriteLine(ColorVm.ColorText);
    //  if (e.PropertyName == "Color")
    //    Brush = new SolidColorBrush(ColorVm.Color ?? Colors.Black);
    //  else if (e.PropertyName == "ColorText")
    //    Road["Color"] = ColorVm.ColorText;
    //}

    public static CRoute Make(Road road, RoadsViewModel roadsVm)
    {
      var points = new List<PointLatLng>();
      foreach (var point in (IList)road["Route"])
        points.Add(LatLngParser.Parse(point));

      road["Route"] = points;

      return new CRoute((IEnumerable<PointLatLng>)road["Route"], roadsVm, road);
    }

    public override void RegenerateShape(GMapControl map)
    {
      base.RegenerateShape(map);
      var path = (Path)Shape;
      if (path == null)
        return;

      Brush = Road["Color"];

      path.Stroke = (SolidColorBrush)Brush;
      path.StrokeThickness = 5;
      path.ToolTip = Road.Name + " " + Road.Length + "km";
      path.IsHitTestVisible = true;
      path.ContextMenu = contextMenu;
    }
  }
}
