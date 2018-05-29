namespace MapConfigure.ViewModels
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using Core.Data;
  using Core.Entities;

  public class RoutesViewModel : ViewModelBase
  {
    public const string GROUP_NONE = "Откл";
    public const string GROUP_NODE = "По городам/пунктам";
    public const string GROUP_ROUTE = "По маршрутам";

    public string[] GroupOptions => new[] { GROUP_NONE, GROUP_NODE, GROUP_ROUTE };
    public string GroupOption
    {
      get => groupOption;
      set => Set(ref groupOption, value);
    }
    private string groupOption;
    public List<RoadStats> Routes => new List<RoadStats>()
    {
      new RoadStats{From = "Atrau",To = "sd"},
      new RoadStats{From = "Орал",To = "Ural"}
    };

    public RoutesViewModel() { }
  }

}
