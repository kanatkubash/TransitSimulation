namespace MapConfigure.ViewModels
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Windows.Input;
  using System.Windows.Media;
  using Commands;
  using Core.Entities;
  using GMap.NET;
  using GMap.NET.WindowsPresentation;
  using Services;

  public class RoadViewModel : ViewModelBase
  {
    public Road Road { get; }
    public RoadsViewModel RoadsVm { get; }
    public Geometry Geometry
    {
      get => geometry;
      set => Set(ref geometry, value);
    }
    private Geometry geometry;
    public ICommand RemoveRoadCommand
    {
      get => removeRoadCommand;
      set => Set(ref removeRoadCommand, value);
    }
    private ICommand removeRoadCommand;
    public ICommand HideRoadCommand
    {
      get => hideRoadCommand;
      set => Set(ref hideRoadCommand, value);
    }
    private ICommand hideRoadCommand;

    public RoadViewModel(RoadsViewModel roadsVm, Road road)
    {
      Road = road;
      RoadsVm = roadsVm;
      RemoveRoadCommand = new RemoveRoad(roadsVm);
    }

  }
}

