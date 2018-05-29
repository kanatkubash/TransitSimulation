using MapConfigure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapConfigure.ViewModels
{
  class SettingsViewModel : ViewModelBase
  {
    public bool IsOpen
    {
      get => isOpen;
      set => IsClosed = !Set(ref isOpen, value);
    }
    private bool isOpen;
    public bool IsClosed
    {
      get => isClosed;
      private set => Set(ref isClosed, value);
    }
    private bool isClosed = true;
    public ICommand ToggleCommand { get; }
    public MapControlViewModel MapViewModel { get; }
    public RoutesViewModel RouteSettingsVm { get; }

    public SettingsViewModel(MapControlViewModel mapVm, RoutesViewModel routeSettingsVm)
    {
      this.MapViewModel = mapVm;
      this.RouteSettingsVm = routeSettingsVm;
      this.ToggleCommand = new ToggleSettings(this);
    }
  }
}
