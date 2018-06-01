namespace MapConfigure.Commands
{
  using System;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using Core.Entities;
  using ViewModels;

  class RemoveNode : ICommand
  {
    private NodesViewModel nodesVm;
    private RoutesViewModel routesVm;
    private RoadsViewModel roadsVm;

    public RemoveNode(NodesViewModel nodesVm, RoutesViewModel routesVm, RoadsViewModel roadsVm)
      => (this.nodesVm, this.routesVm, this.roadsVm) = (nodesVm, routesVm, roadsVm);

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      if (parameter == null)
        return;
      var node = (Node)parameter;
      if (routesVm.Routes.Any(r => r.From == node.Name || r.To == node.Name))
      {
        var toRemoveRoads = roadsVm.Roads.Where(r => r.From == node || r.To == node);
        var count = toRemoveRoads.Count();

        if (count > 0)
        {
          if (MessageBox.Show($"Будет удален данный узел и {count} дорог. Удалить?", "",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
          {
            toRemoveRoads.ToList().ForEach(r => roadsVm.Roads.Remove(r));
            RemoveNodeAction(parameter);
          }
        }
        else
          RemoveNodeAction(parameter);
      }
      else
        MessageBox.Show("Нельзя удалить так как данный пункт имеет маршруты");
    }

    private void RemoveNodeAction(object node) => nodesVm.Nodes.Remove((Node)node);

    public event EventHandler CanExecuteChanged;
  }
}
