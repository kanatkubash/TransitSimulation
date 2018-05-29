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

    public RemoveNode(NodesViewModel nodesVm, RoutesViewModel routesVm)
      => (this.nodesVm, this.routesVm) = (nodesVm, routesVm);

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      if (parameter == null)
      {
        return;
      }
      var node = (Node)parameter;
      if (routesVm.Routes.FirstOrDefault(r => r.From == node.Name || r.To == node.Name) == null)
        nodesVm.Nodes.Remove((Node)parameter);
      else
        MessageBox.Show("Нельзя удалить так как данный пункт имеет маршруты");
    }

    public event EventHandler CanExecuteChanged;
  }
}
