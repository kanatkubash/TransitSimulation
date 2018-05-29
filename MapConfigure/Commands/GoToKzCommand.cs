namespace MapConfigure.Commands
{
  using GMap.NET;
  using GMap.NET.WindowsPresentation;
  using MapConfigure.ViewModels;
  using System;
  using System.Windows.Input;

  class GoToKzCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;
    private PointLatLng kzCenter;

    public GoToKzCommand(PointLatLng kzCenter) => this.kzCenter = kzCenter;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      ((GMapControl)parameter).CenterPosition = default(PointLatLng);
      ((GMapControl)parameter).CenterPosition = kzCenter;
    }
  }
}
