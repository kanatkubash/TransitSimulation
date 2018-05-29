namespace MapConfigure.Commands
{
  using System;
  using System.Windows;
  using System.Windows.Input;
  using MaterialDesignThemes.Wpf;

  /// <summary>
  /// Closes modal
  /// </summary>
  class ToggleModal : ICommand
  {
    private Action<object> endAction;

    public ToggleModal(Action<object> endAction) => this.endAction = endAction;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      DialogHost.CloseDialogCommand.Execute(null, (IInputElement)parameter);
      endAction?.Invoke(parameter);
    }

    public event EventHandler CanExecuteChanged;
  }
}
