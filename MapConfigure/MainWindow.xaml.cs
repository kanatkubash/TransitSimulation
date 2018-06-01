using GMap.NET;
using GMap.NET.WindowsPresentation;
using MapConfigure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapConfigure
{
  using System.Collections;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.IO;
  using Components;
  using Core.Data;
  using Core.Entities;
  using Data;
  using Newtonsoft.Json;
  using Path = System.Windows.Shapes.Path;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private NodesViewModel NodesVm;
    private RoadsViewModel RoadsVm;
    private MapControlViewModel MapVm;
    private SettingsViewModel SettingsVm;
    private RoutesViewModel RoutesVm;

    private void InitMapControl()
    {
      GMaps.Instance.Mode = AccessMode.ServerAndCache;
      MapControl.DragButton = MouseButton.Middle;
      this.Closing += (_, __) =>
      {
        MapControl.Manager.CancelTileCaching();
        MapControl.Dispose();
      };
      foreach (var node in NodesVm.Nodes)
      {
        MapControl.Markers.Add(new MapNodeVm(node, NodesVm, RoutesVm, RoadsVm));
      }

      foreach (var road in RoadsVm.Roads)
      {
        var cRoute = CRoute.Make(road, RoadsVm);
        cRoute.PropertyChanged += CRouteHideHandler;
        MapControl.Markers.Add(cRoute);
      }
    }

    private void CRouteHideHandler(object sender, PropertyChangedEventArgs p)
    {
      var cRoute = (CRoute)sender;
      if (p.PropertyName == "IsVisible")
        MapVm.HiddenRoutes += !cRoute.IsVisible ? 1 : -1;
    }

    private void SetContexts()
    {
      NodesVm = new NodesViewModel(new JsonRepo<Node>());
      RoadsVm = new RoadsViewModel(new JsonRepo<Road>());

      NodesVm.NodesChanged += (a, b) =>
      {
        switch (b.Action)
        {
          case NotifyCollectionChangedAction.Remove:
            foreach (Node item in b.OldItems)
            {
              var marker = MapControl.Markers.OfType<MapNodeVm>().First(m => m.Node == item);
              MapControl.Markers.Remove(marker);
            }
            break;
          case NotifyCollectionChangedAction.Add:
            foreach (Node item in b.NewItems)
              MapControl.Markers.Add(new MapNodeVm(item, NodesVm, RoutesVm, RoadsVm));
            break;
        }
      };
      RoadsVm.RoadsChanged += (a, b) =>
      {
        switch (b.Action)
        {
          case NotifyCollectionChangedAction.Remove:
            foreach (Road item in b.OldItems)
            {
              var marker = MapControl.Markers.OfType<CRoute>().First(m => m.Road == item);
              MapControl.Markers.Remove(marker);
            }
            break;
          case NotifyCollectionChangedAction.Add:
            foreach (Road item in b.NewItems)
              MapControl.Markers.Add(CRoute.Make(item, RoadsVm));
            break;
        }
      };

      RoutesVm = new RoutesViewModel(new JsonRepo<TransportStats>(), NodesVm.Nodes, RoadsVm.Roads);
      MapVm = new MapControlViewModel(this.Resources, NodesVm);
      SettingsVm = new SettingsViewModel(MapVm, RoutesVm);
      MapControl.DataContext = MapVm;
      SettingsOpenBtn.DataContext = SettingsVm;
      MainMenuControl.DataContext = SettingsVm;
      NodeNameText.DataContext = NodesVm;
      this.DataContext = MapVm;
    }

    public MainWindow()
    {
      InitializeComponent();
      SetContexts();
      InitMapControl();
      return;
      GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
      var x = new GMap.NET.WindowsPresentation.GMapControl();
      x.MapProvider = GMap.NET.MapProviders.YandexMapProvider.Instance;
      x.Loaded += X_Loaded;
      x.CanDragMap = true;
      x.MouseDoubleClick += (a, b) =>
      {
        x.MapProvider = GMap.NET.MapProviders.YandexHybridMapProvider.Instance;
        var pos = b.GetPosition(this);
        this.Title = x.FromLocalToLatLng((int)pos.X, (int)pos.Y).ToString();
        var l = x.FromLocalToLatLng((int)pos.X, (int)pos.Y);
        x.Markers.Add(new GMap.NET.WindowsPresentation.GMapMarker(l));
        x.Markers.Last().Shape = new Button()
        {
          Width = 10,
          Height = 10,
          Background = Brushes.BlueViolet,
          Content = "X",
          Margin = new Thickness(-5, -5, 0, 0),
          IsHitTestVisible = false
        };
        //var s = new GMapRoute((x.MapProvider as RoutingProvider).GetRoute(
        //new GMap.NET.PointLatLng(56.292333, 44.080159),
        //new GMap.NET.PointLatLng(90, 21),
        //false,
        //false,
        //5
        //).Points);
        var ax = (GMap.NET.MapProviders.GoogleMapProvider.Instance as GMap.NET.MapProviders.GoogleMapProvider).GetDirections(out GDirections ca,
        new GMap.NET.PointLatLng(44.212955, 80.400367),
        new GMap.NET.PointLatLng(51.230645, 51.366255), false, false, false, false, false);
        var ss = new GMapRoute(ca.Route);
        x.Markers.Add(ss);
        (ss.Shape as Path).Stroke = new SolidColorBrush(Colors.MediumVioletRed);
        var r = new Random();
        for (int i = 0; i < 2000; i++)
        {
          var q = new byte[3];
          r.NextBytes(q);
          x.Markers.Add(new GMapMarker(new PointLatLng(r.NextDouble() * 180, r.NextDouble() * 180)) { Shape = new Ellipse() { Width = r.Next(2, 5), Height = r.Next(3, 6), Fill = new SolidColorBrush(Color.FromArgb(255, q[0], q[1], q[2])) } });
        }
        //x.Markers.Last().Shape = new Path()
        //{
        //  Data = Geometry.Parse("M 100,200 C 100,25 400,350 400,175 H 280"),
        //  StrokeThickness = 2,
        //  Stroke = new BrushConverter().ConvertFrom("#ff00aa") as SolidColorBrush
        //};
      };
      MainGrid.Children.Add(x);
      this.Closing += (e, ex) =>
      {
        x.Manager.CancelTileCaching();
        x.Dispose();
        //Application.Current.Shutdown(0);
      };
    }

    private void X_Loaded(object sender, RoutedEventArgs e)
    {
      //return;
      GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
      var mapView = sender as GMap.NET.WindowsPresentation.GMapControl;
      mapView.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
      mapView.MinZoom = 5;
      mapView.MaxZoom = 10;
      mapView.CenterPosition = new GMap.NET.PointLatLng(47.729578, 67.232466);
      //mapView.Markers.Add(new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(47.6, 67.5)));
      //mapView.BoundsOfMap = new GMap.NET.RectLatLng(56.292333, 44.080159, 44 - 21, 98);
      //mapView.CenterPosition = new GMap.NET.PointLatLng(10.5, 10.2);
      //mapView.BoundsOfMap = new GMap.NET.RectLatLng(10.292333, 10.080159, 10, 10);
      // whole world zoom
      mapView.Zoom = 15;
      // lets the map use the mousewheel to zoom
      mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
      // lets the user drag the map
      mapView.CanDragMap = true;
      // lets the user drag the map with the left mouse button
      mapView.DragButton = MouseButton.Left;

      return;
      throw new NotImplementedException();
    }
  }
}
