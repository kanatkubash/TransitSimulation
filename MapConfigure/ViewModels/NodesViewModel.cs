namespace MapConfigure.ViewModels
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.Linq;
  using System.Windows.Input;
  using Commands;
  using Core.Entities;
  using Data;

  public class NodesViewModel : ViewModelBase
  {
    private IRepository<Node> nodeRepo;
    public ObservableCollection<Node> Nodes
    {
      get => nodes;
      private set => Set(ref nodes, value);
    }
    private ObservableCollection<Node> nodes;
    public Node ChosenNode
    {
      get => chosenNode;
      set => Set(ref chosenNode, value);
    }
    private Node chosenNode;
    public ICommand EditNodeFinishCommand
    {
      get => editNodeFinishCommand;
      set => Set(ref editNodeFinishCommand, value);
    }
    private ICommand editNodeFinishCommand;

    public NodesViewModel(IRepository<Node> nodeRepo)
    {
      this.nodeRepo = nodeRepo;
      EditNodeFinishCommand = new EditNodeFinish(this);
      Nodes = new ObservableCollection<Node>(nodeRepo.LoadAll());
      Nodes.CollectionChanged += (a, b) =>
      {
        nodeRepo.SaveAll(Nodes);
        NodesChanged?.Invoke(a, b);
      };
    }

    public event NotifyCollectionChangedEventHandler NodesChanged;
  }
}
