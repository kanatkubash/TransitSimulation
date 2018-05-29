namespace MapConfigure.Commands
{
  using System;
  using System.Windows.Input;
  using Core.Entities;
  using ViewModels;

  public class EditNode : ICommand
  {
    private NodesViewModel nodesVm;

    public EditNode(NodesViewModel nodesVm) => this.nodesVm = nodesVm;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      nodesVm.ChosenNode = (Node)parameter;
    }

    public event EventHandler CanExecuteChanged;
  }
}
