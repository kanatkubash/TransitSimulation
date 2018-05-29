namespace MapConfigure.ViewModels
{
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using Core.Entities;
  using Data;

  public class RoadsViewModel : ViewModelBase
  {
    public IRepository<Road> RoadRepo;
    public ObservableCollection<Road> Roads =>
      roads ?? (roads = new ObservableCollection<Road>(RoadRepo.LoadAll()));
    private ObservableCollection<Road> roads;

    public RoadsViewModel(IRepository<Road> roadRepo)
    {
      RoadRepo = roadRepo;
      Roads.CollectionChanged += (a, b) =>
      {
        RoadRepo.SaveAll(Roads);
        RoadsChanged?.Invoke(a, b);
      };
    }

    public event NotifyCollectionChangedEventHandler RoadsChanged;
  }
}
