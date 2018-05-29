namespace MapConfigure.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Media;
  using System.Windows.Shapes;
  using Commands;
  using Components;
  using Core.Entities;
  using GMap.NET.WindowsPresentation;
  using ViewModels;

  public class MapNodeVm : GMapMarker
  {
    private const double WIDTH = 15;

    public ICommand RemoveNodeCommand { get; set; }
    public ICommand FindRoadCommand { get; set; }
    private NodesViewModel nodesVm;
    private RoutesViewModel routesVm;
    private RoadsViewModel roadsVm;

    public Node Node { get; }
    public IEnumerable<Node> Nodes => nodesVm.Nodes.Where(n => n != Node);

    public MapNodeVm(Node node, NodesViewModel nodesVm, RoutesViewModel routesVm,
      RoadsViewModel roadsVm) : base(node.LatLng)
    {
      Node = node;
      this.nodesVm = nodesVm;
      this.routesVm = routesVm;
      this.roadsVm = roadsVm;

      RemoveNodeCommand = new RemoveNode(nodesVm, routesVm);
      FindRoadCommand = new FindRoad(Node, roadsVm);
      nodesVm.NodesChanged += (a, b) => this.OnPropertyChanged(new PropertyChangedEventArgs("Nodes"));
      Shape = new MapNodeShape(this);
    }

    protected new void OnPropertyChanged(PropertyChangedEventArgs name)
    {
      base.OnPropertyChanged(name);
    }

    public double Len => WIDTH;
  }
}
