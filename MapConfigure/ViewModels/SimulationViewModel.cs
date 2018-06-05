using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Data;
using Core.Entities;
using Core.Helpers;
using MapConfigure.Data;
using MapConfigure.Helpers;

namespace MapConfigure.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Timers;
  using System.Windows.Input;
  using System.Windows.Threading;
  using Commands;
  using LiveCharts;
  using LiveCharts.Configurations;

  public class SimulationViewModel : ViewModelBase
  {
    #region mappers

    public CartesianMapper<SealStats> InquiryMapper;
    public CartesianMapper<SealStats> StockMapper;
    public static CartesianMapper<SealStats> ReuseMapper;
    public static CartesianMapper<SealStats> IncrementMapper;

    #endregion

    static SimulationViewModel()
    {
      //InquiryMapper = Mappers.Xy<SealStats>().X(s => 1).Y(s => 2);
    }

    private Task simulationTask;
    private CancellationTokenSource taskTokenSource;
    public ObservableCollection<SealStats> SealStats { get; private set; }
    public bool IsEquidistribute
    {
      get => isEquidistribute;
      set => Set(ref isEquidistribute, value);
    }
    private bool isEquidistribute = true;
    public bool IsRunning
    {
      get => isRunning;
      set => Set(ref isRunning, value);
    }
    private bool isRunning;
    public bool IsEnabled
    {
      get => isEnabled;
      set => Set(ref isEnabled, value);
    }
    protected Dispatcher Dispatcher = null;
    private bool isEnabled = true;

    public ICommand SimulationToggleCommand => new RelayCommand((g) =>
    {
      IsEnabled = false;
      if (IsRunning)
      {
        taskTokenSource.Cancel();
        Console.WriteLine("Завершаем симуляцию...");
      }
      else
      {
        taskTokenSource = new CancellationTokenSource();
        var token = taskTokenSource.Token;
        Simulation simulation = null;
        simulationTask = Task.Run(() =>
          {
            var tempTask = Task.Run(() =>
            {
              Thread.Sleep(50);
              IsRunning = true;
              IsEnabled = true;
            });
            tempTask.ContinueWith(_ => tempTask.Dispose());
            /// simulation
            Console.WriteLine("START");
            IRandom distributer = null;
            if (IsEquidistribute)
              distributer = new EqualDistribution();
            else
              distributer = new PseudoRandom();
            simulation = new Simulation(
              new JsonRepo<TransportStats>(),
              RoadStatsMemoryRepo.Make(new JsonRepo<Road>()),
              distributer);
            simulation.SetUp();
            simulation.Start(AvgSpeed, SealCountsPercentage.Skip(1).ToArray(), token);
            Console.WriteLine("END");
          }, token)
          .ContinueWith((task) =>
          {
            Thread.Sleep(100);
            IsEnabled = true;
            IsRunning = false;
            MaxTruckCount = simulation.TruckCountStatsGatherer.Max;
            Dispatcher.Invoke(() =>
            {
              SealStats.Clear();
              simulation.SealCountStatsGatherer.SealStats.ForEach(s => SealStats.Add(s));
              TruckCounts.Clear();
              TruckCounts.AddRange(simulation.TruckCountStatsGatherer.TruckCounts.Select((tC, i) => new { tC, i })
                .GroupBy(a => a.i / 1000).Select(gC => gC.Sum(t => t.tC) / 1000));
            });

            taskTokenSource.Dispose();
          });
      }
    });

    public ICommand PlotChartCommand => new RelayCommand((param) =>
    {
      if (this.Selected == null)
        return;
      SealStatsIncr.Clear();
      //SealStatsInquiry.Clear();
      //SealStatsReuse.Clear();
      //SealStatsStock.Clear();
      var index = (string)param;
      int[] selected = null;
      if (index == "0")
        selected = Selected.Stock;
      else if (index == "1")
        selected = Selected.Inquiries;
      else if (index == "2")
        selected = Selected.Reuses;
      else if (index == "3")
        selected = Selected.Increments;

      SealStatsIncr.AddRange(selected);
      //SealStatsReuse.AddRange(Selected.Reuses);
      //SealStatsStock.AddRange(Selected.Stock);
      //SealStatsInquiry.AddRange(Selected.Inquiries);
    });
    public ObservableCollection<int> SealCountsPercentage => sealCountsPercentage;
    private ObservableCollection<int> sealCountsPercentage
      = new ObservableCollection<int> { 0, 25, 25, 25, 25 };
    public int AvgSpeed
    {
      get => avgSpeed;
      set => Set(ref avgSpeed, value);
    }
    private int avgSpeed;
    public int MaxTruckCount
    {
      get => maxTruckCount;
      private set => Set(ref maxTruckCount, value);
    }
    private int maxTruckCount;
    public ChartValues<int> TruckCounts
    {
      get => truckCounts;
      set => Set(ref truckCounts, value);
    }
    private ChartValues<int> truckCounts;
    public SealStats Selected
    {
      get => selected;
      set => Set(ref selected, value);
    }
    public SealStats selected;
    #region Seal Stats
    public ChartValues<int> SealStatsInquiry
    {
      get => sealStatsInquiry;
      set => Set(ref sealStatsInquiry, value);
    }
    private ChartValues<int> sealStatsInquiry;
    public ChartValues<int> SealStatsStock
    {
      get => sealStatsStock;
      set => Set(ref sealStatsStock, value);
    }
    private ChartValues<int> sealStatsStock;
    public ChartValues<int> SealStatsReuse
    {
      get => sealStatsReuse;
      set => Set(ref sealStatsReuse, value);
    }
    private ChartValues<int> sealStatsReuse;
    public ChartValues<int> SealStatsIncr
    {
      get => sealStatsIncr;
      set => Set(ref sealStatsIncr, value);
    }
    private ChartValues<int> sealStatsIncr;
    #endregion

    public SimulationViewModel(Dispatcher dispatcher)
    {
      InitSliderhandler();
      SealStats = new ObservableCollection<SealStats>();
      TruckCounts = new ChartValues<int>();
      SealStatsIncr = new ChartValues<int>();
      SealStatsInquiry = new ChartValues<int>();
      SealStatsReuse = new ChartValues<int>();
      SealStatsStock = new ChartValues<int>();
      Dispatcher = dispatcher;
    }

    private void InitSliderhandler()
    {
      int GetNextIndex(int index, bool zeroCheck)
      {
        var initial = index;
        var percentage = 100;

        do
        {
          index = index == 4 ? 1 : index + 1;
          percentage = SealCountsPercentage[index];
        } while (zeroCheck && percentage == 0 && initial != index);

        return index;
      }

      var entered = false;
      SealCountsPercentage.CollectionChanged += (a, b) =>
      {
        if (entered)
          return;
        if (b.Action == NotifyCollectionChangedAction.Replace)
        {
          entered = true;
          var nextIndex = GetNextIndex(b.NewStartingIndex, true);
          var delta = (int)b.NewItems[0] - (int)b.OldItems[0];

          if (nextIndex != b.NewStartingIndex)
            SealCountsPercentage[nextIndex] += -delta;
          else
            SealCountsPercentage[GetNextIndex(b.NewStartingIndex, false)] += -delta;
          PropertyChange(this, new PropertyChangedEventArgs("SimulationToggleCommand"));
          entered = false;
        }
      };
    }
  }
}
