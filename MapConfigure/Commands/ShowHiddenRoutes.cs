namespace MapConfigure.Commands
{
  using System;
  using System.Linq;
  using System.Windows.Documents;
  using System.Windows.Input;
  using Components;
  using GMap.NET.WindowsPresentation;

  public class ShowHiddenRoutes : ICommand
  {
    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      var gmapControl = (GMapControl)parameter;
      gmapControl.Markers.OfType<CRoute>()?.ForEach(c => c.IsVisible = true);
    }

    public event EventHandler CanExecuteChanged;
  }
}
