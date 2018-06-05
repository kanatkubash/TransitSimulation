namespace MapConfigure.Commands
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Input;
  using Core.Entities;
  using ViewModels;

  public class RouteErrorCheck : ICommand
  {
    private readonly RoutesViewModel routesVm;
    private readonly IList<Road> roads;

    public RouteErrorCheck(RoutesViewModel routesVm, IList<Road> roads)
    {
      (this.routesVm, this.roads) = (routesVm, roads);
    }

    public bool CanExecute(object parameter) => !routesVm.IsRealEdit;

    public void Execute(object parameter)
    {
      routesVm.RouteErrors.Clear();
      var roadComputedNames = roads.Select(r => r.ComputedName).Distinct();
      routesVm.Routes.ForEach(ts =>
      {
        if (!roadComputedNames.Contains(ts.ComputedName))
          routesVm.RouteErrors.Add($"{ts} : дорога не построена");
      });
      if (routesVm.RouteErrors.Count == 0)
        routesVm.RouteErrors.Add("OK");
    }

    public event EventHandler CanExecuteChanged;
  }
}
