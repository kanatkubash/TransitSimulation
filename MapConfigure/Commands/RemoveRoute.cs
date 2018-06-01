namespace MapConfigure.Commands
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;
  using Core.Data;

  public class RemoveRoute : ICommand
  {
    private IList<TransportStats> routes;

    public bool CanExecute(object parameter) => true;

    public RemoveRoute(IList<TransportStats> routes) => this.routes = routes;

    public void Execute(object parameter)
    {
      IEnumerable<TransportStats> toRemoveRoutes = ((IList)parameter).Cast<TransportStats>();
      toRemoveRoutes?.ToList().ForEach(route => routes.Remove(route));
    }

    public event EventHandler CanExecuteChanged;
  }
}
