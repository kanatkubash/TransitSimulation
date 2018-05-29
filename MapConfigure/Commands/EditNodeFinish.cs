namespace MapConfigure.Commands
{
  using System;
  using System.Linq;
  using System.Windows.Input;
  using ViewModels;

  class EditNodeFinish : ICommand
  {
    private NodesViewModel nodesVm;

    public bool CanExecute(object parameter) => true;

    public EditNodeFinish(NodesViewModel vm) => nodesVm = vm;

    public void Execute(object parameter)
    {
      var node = nodesVm.ChosenNode;
      if (node.Id == 0)
      {
        if (!string.IsNullOrWhiteSpace(node.Name))
          nodesVm.Nodes.Add(node);
      }
      else
      {
        nodesVm.Nodes.First(n => n.Id == node.Id).Name = node.Name;
      }
    }

    public event EventHandler CanExecuteChanged;
  }
}
