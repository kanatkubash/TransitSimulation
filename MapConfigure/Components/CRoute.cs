namespace MapConfigure.Components
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Media;
  using System.Windows.Media.Animation;
  using System.Windows.Shapes;
  using Commands;
  using Core.Entities;
  using GMap.NET;
  using GMap.NET.WindowsPresentation;
  using Services;
  using ViewModels;

  /// <summary>
  /// GmapRoute with context menu and tooltip support
  /// Because of the fact GmapRoute uses StreamGeometry for path building rather than PathGeometry
  /// pure MVVM practice is impossible. So we have to use code behind
  /// </summary>
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
    public bool IsVisible
    {
      get => isVisible;
      set
      {
        var shouldFire = isVisible != value;
        isVisible = value;
        if (shouldFire)
          OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
      }
    }
    private bool isVisible;
    private ContextMenu contextMenu;
    private bool handlerAdded;
    private int zindex;

    protected CRoute(IEnumerable<PointLatLng> points, RoadsViewModel roadVm, Road road)
      : base(points)
    {
      (RoadsVm, Road) = (roadVm, road);
      RoadVm = new RoadViewModel(RoadsVm, road);
      IsVisible = true;

      contextMenu = new ContextMenu()
      {
        Items =
        {
          new MenuItem()
          {
            Header = "Удалить",
            Command = RoadVm.RemoveRoadCommand,
            CommandParameter = Road,
          },
          new MenuItem()
          {
            Header = "Скрыть",
            Command = new RelayCommand(HideAction),
            CommandParameter = this,
          },
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

    protected static Action<object> HideAction = param =>
    {
      var route = (CRoute)param;
      route.IsVisible = false;
    };

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
      if (Shape != null)
        Shape.Effect = null;

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

      if (!handlerAdded)
      {
        DependencyPropertyDescriptor.FromProperty(Path.IsMouseOverProperty, typeof(Path))
          .AddValueChanged(path, this.Path_IsMouseCapturedChanged);
        handlerAdded = true;
        SetBinding(path);
      }
    }

    private void Path_IsMouseCapturedChanged(object sender, EventArgs e)
    {
      var path = (Path)sender;
      path.StrokeThickness = path.IsMouseOver ? 6 : 5;
    }

    private void SetBinding(UIElement element)
    {
      var binding = new Binding("IsVisible");
      binding.Source = this;
      binding.Converter = new BooleanToVisibilityConverter();
      BindingOperations.SetBinding(element, UIElement.VisibilityProperty, binding);
    }
  }
}
