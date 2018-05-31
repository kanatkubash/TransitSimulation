namespace MapConfigure.Commands
{
  using System;
  using System.Windows.Input;

  public class RelayCommand : ICommand
  {
    private Action<object> action;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => action?.Invoke(parameter);

    public RelayCommand(Action<object> action) => this.action = action;

    public event EventHandler CanExecuteChanged;
  }
}
