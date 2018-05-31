namespace MapConfigure.ViewModels
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.Windows.Documents;
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
      set => Set(ref groupOption, value);
    }
    private string groupOption;
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
    public ObservableCollection<Node> nodes;

    public IList<Node> N => new[] { new Node("aed", 1), };

    public RoutesViewModel(IRepository<TransportStats> repo, ObservableCollection<Node> nodes)
    {
      repository = repo;
      this.nodes = nodes;

      Routes = new ObservableCollection<TransportStats>(repository.LoadAll());
      Routes.CollectionChanged += (a, b) =>
      {
        repository.SaveAll(Routes);
        RoutesChanged?.Invoke(a, b);
      };
    }

    public event NotifyCollectionChangedEventHandler RoutesChanged;
  }

}
