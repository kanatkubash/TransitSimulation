namespace MapConfigure.ViewModels
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Linq;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Input;
  using Commands;
  using Converters;
  using Core.Data;
  using Core.Entities;
  using Data;

  public class RoutesViewModel : ViewModelBase
  {
    public const string GROUP_NONE = "Откл";
    public const string GROUP_NODE = "По городам/пунктам";
    public const string GROUP_ROUTE = "По маршрутам";

    public string[] GroupOptions => new[] { GROUP_NONE, GROUP_NODE, GROUP_ROUTE };
    public string GroupOption
    {
      get => groupOption;
      set
      {
        Set(ref groupOption, value);
        Regroup(value);
      }
    }
    private string groupOption = GROUP_NONE;
    public bool IsEditMode
    {
      get => isEditMode;
      set
      {
        IsReadMode = !value;
        Set(ref isEditMode, value);
      }
    }
    private bool isEditMode;
    public bool IsReadMode
    {
      get => isReadMode;
      set => Set(ref isReadMode, value);
    }
    private bool isReadMode = true;
    private IRepository<TransportStats> repository;
    public ObservableCollection<TransportStats> Routes
    {
      get => routes;
      set => Set(ref routes, value);
    }
    private ObservableCollection<TransportStats> routes;
    public ObservableCollection<Node> Nodes
    {
      get => nodes;
      set => Set(ref nodes, value);
    }
    private ObservableCollection<Node> nodes;
    private IList<Road> roads;
    public ListCollectionView RoutesView
      => (ListCollectionView)CollectionViewSource.GetDefaultView(Routes);
    public ICommand RemoveRoutesCommand
    {
      get => removeRoutesCommand;
      set => Set(ref removeRoutesCommand, value);
    }
    private ICommand removeRoutesCommand;
    public ICommand EditCommand => new RelayCommand(_ =>
    {
      Routes.Clear();
      IsEditMode = false;
      repository.LoadAll().ForEach(ts => Routes.Add(ts));
    });
    public ICommand SaveCommand => new RelayCommand(_ =>
    {
      if (IsRealEdit)
      {
        MessageBox.Show("Исправьте ошибки");
        return;
      }
      repository.SaveAll(Routes);
      IsEditMode = false;
    });
    public ICommand AddReverseRouteCommand => new RelayCommand(item =>
    {
      var trStat = (TransportStats)(item as IList)[0];
      var newTrStat = trStat.Reverse();
      if (Routes.Any(r => r.From == newTrStat.From && r.To == newTrStat.To))
      {
        MessageBox.Show("Обратный маршрут уже существует");
        return;
      }
      Routes.Add(newTrStat);
      SaveCommand.Execute(null);
    });

    public ObservableCollection<string> RouteErrors
    {
      get => routeErrors;
      set => Set(ref routeErrors, value);
    }
    private ObservableCollection<string> routeErrors;
    public ICommand RouteErrorCheckCommand => new RouteErrorCheck(this, roads);

    public bool IsRealEdit => RoutesView.IsAddingNew || RoutesView.IsEditingItem;

    public RoutesViewModel(
      IRepository<TransportStats> repo,
      ObservableCollection<Node> nodes,
      IList<Road> roads)
    {
      repository = repo;
      Nodes = nodes;
      this.roads = roads;
      RouteErrors = new ObservableCollection<string>();
      Routes = new ObservableCollection<TransportStats>();
      repository.LoadAll().ForEach(ts => Routes.Add(ts));
      Routes.CollectionChanged += (a, b) =>
      {
        if (b.Action != NotifyCollectionChangedAction.Remove)
          return;
        repository.SaveAll(Routes);
        RoutesChanged?.Invoke(a, b);
      };
      RemoveRoutesCommand = new RemoveRoute(Routes);

    }

    private void Regroup(string groupOpt)
    {
      RoutesView.GroupDescriptions.Clear();

      switch (groupOpt)
      {
        case GROUP_NODE:
          RoutesView.GroupDescriptions.Add(new PropertyGroupDescription("From"));
          break;
        case GROUP_ROUTE:
          RoutesView.GroupDescriptions.Add(new PropertyGroupDescription(".", new TrStatsToRouteConverter()));
          break;
      }
    }

    public event NotifyCollectionChangedEventHandler RoutesChanged;
  }

}
