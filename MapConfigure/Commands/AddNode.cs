namespace MapConfigure.Commands
{
  using GMap.NET.WindowsPresentation;
  using System;
  using System.Windows;
  using System.Windows.Input;
  using Core.Entities;
  using ViewModels;

  class AddNode : ICommand
  {
    public bool CanExecute(object parameter) => true;
    public MapControlViewModel MapViewModel { get; }
    public NodesViewModel NodesViewModel { get; }
    private readonly Action<object> endAction;

    public AddNode(MapControlViewModel mapVm, NodesViewModel nodeVm,
      Action<object> endAction = null)
      => (MapViewModel, NodesViewModel, this.endAction) = (mapVm, nodeVm, endAction);

    public void Execute(object parameter)
    {
      var mousePos = Mouse.GetPosition((IInputElement)parameter);
      var mapControl = (GMapControl)parameter;
      var latLng = mapControl.FromLocalToLatLng((int)mousePos.X, (int)mousePos.Y);
      MapViewModel.ChosenPoint = latLng;
      NodesViewModel.ChosenNode = new Node("") { LatLng = latLng };
      endAction?.Invoke(parameter);
    }

    public event EventHandler CanExecuteChanged;
  }
}
