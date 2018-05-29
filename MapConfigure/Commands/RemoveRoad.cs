namespace MapConfigure.Commands
{
  using System;
  using System.Windows.Input;
  using Core.Entities;
  using ViewModels;

  public class RemoveRoad : ICommand
  {
    public RoadsViewModel roadsVm;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      var road = (Road)parameter;
      roadsVm.Roads.Remove(road);
    }

    public RemoveRoad(RoadsViewModel roadsVm) => this.roadsVm = roadsVm;

    public event EventHandler CanExecuteChanged;
  }
}
