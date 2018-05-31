using System.Diagnostics;

namespace MapConfigure.ViewModels
{
  using GMap.NET;
  using GMap.NET.MapProviders;
  using Commands;
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using Components;
  using Core.Entities;
  using GMap.NET.WindowsPresentation;
  using MaterialDesignThemes.Wpf;

  class MapControlViewModel : ViewModelBase
  {
    public NodesViewModel NodesVm { get; set; }
    public GMapProvider Provider
    {
      get => gMapProvider;
      set => Set(ref gMapProvider, value);
    }
    private GMapProvider gMapProvider = GMapProviders.GoogleHybridMap;
    public Dictionary<string, GMapProvider> ProviderList { get; }
    public PointLatLng CenterPoint
    {
      get => centerPoint;
      set => Set(ref centerPoint, value);
    }
    private PointLatLng centerPoint;
    public ICommand GoToKz
    {
      get => goToKz;
      set => Set(ref goToKz, value);
    }
    private ICommand goToKz;
    public ICommand AddNodeCommand
    {
      get => addNodeCommand;
      set => Set(ref addNodeCommand, value);
    }
    private ICommand addNodeCommand;
    public PointLatLng ChosenPoint
    {
      get => chosenPoint;
      set => Set(ref chosenPoint, value);
    }
    private PointLatLng chosenPoint;
    public ICommand ToggleModalCommand
    {
      get => toggleModalCommand;
      set => Set(ref toggleModalCommand, value);
    }
    private ICommand toggleModalCommand;
    public ICommand ShowHiddenRoutesCommand
    {
      get => showHiddenRoutesCommand;
      set => Set(ref showHiddenRoutesCommand, value);
    }
    private ICommand showHiddenRoutesCommand;
    public int HiddenRoutes
    {
      get => hiddenRoutes;
      set
      {
        Set(ref hiddenRoutes, value);
        CanHideRoutes = hiddenRoutes != 0;
      }
    }
    private int hiddenRoutes;
    public bool CanHideRoutes
    {
      get => canHideRoutes;
      set => Set(ref canHideRoutes, value);
    }
    private bool canHideRoutes;

    public MapControlViewModel(IDictionary dictionary, NodesViewModel nodesVm)
    {
      CenterPoint = (PointLatLng)dictionary["KZCenter"];
      NodesVm = nodesVm;

      GoToKz = new GoToKzCommand(CenterPoint);
      AddNodeCommand = new AddNode(this, NodesVm, (object parameter) =>
         DialogHost.OpenDialogCommand.Execute(null, (IInputElement)parameter)
      );
      ToggleModalCommand = new ToggleModal(nodesVm.EditNodeFinishCommand.Execute);
      ShowHiddenRoutesCommand = new ShowHiddenRoutes();

      ProviderList = new Dictionary<string, GMapProvider>()
      {
        ["Google"] = GMapProviders.GoogleMap,
        ["Bing Hybrid"] = GMapProviders.BingHybridMap,
        ["Bing"] = GMapProviders.BingMap,
        ["Yandex"] = GMapProviders.YandexMap,
        ["Yandex Hybrid"] = GMapProviders.YandexHybridMap,
      };
    }


  }
}
