namespace MapConfigure.Commands
{
  using System;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using Core.Entities;
  using Services;
  using Unity;
  using ViewModels;

  public class FindRoad : ICommand
  {
    private Node node;
    private RoadsViewModel roadsVm;

    public bool CanExecute(object parameter) => true;

    public FindRoad(Node node, RoadsViewModel roadsVm)
      => (this.node, this.roadsVm) = (node, roadsVm);

    public void Execute(object parameter)
    {
      var toNode = (Node)parameter;
      if (roadsVm.Roads.Count(r => r.To == node && r.From == toNode) > 0
          || roadsVm.Roads.Count(r => r.From == node && r.To == toNode) > 0)
      {
        MessageBox.Show("Указанная дорога уже существует");
        return;
      }
      var dir = new GoogleDirectionFinder().GetDirection(node, toNode);
      if (dir == null)
      {
        MessageBox.Show("Не удалось построить маршрут");
        return;
      }
      var road = new Road(node.Name + "-" + toNode.Name, dir.DistanceValue / 1000.0)
      {
        From = node,
        To = toNode,
      };
      road["Route"] = dir.Route;
      road["Color"] = "#000";
      roadsVm.Roads.Add(road);
    }

    public event EventHandler CanExecuteChanged;
  }
}
