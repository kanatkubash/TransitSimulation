namespace MapConfigure.Commands
{
  using MapConfigure.ViewModels;
  using System;
  using System.Windows.Input;

  class ToggleSettings : ICommand
  {
    public event EventHandler CanExecuteChanged;
    public bool IsOpen { get => ViewModel.IsOpen; }
    public SettingsViewModel ViewModel { get; }

    public ToggleSettings(SettingsViewModel vm) => ViewModel = vm;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => ViewModel.IsOpen = (bool)parameter;
  }
}
