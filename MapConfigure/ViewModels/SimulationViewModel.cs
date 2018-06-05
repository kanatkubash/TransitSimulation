namespace MapConfigure.ViewModels
{
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Timers;
  using System.Windows.Input;
  using Commands;

  public class SimulationViewModel : ViewModelBase
  {
    public bool IsRunning
    {
      get => isRunning;
      set => Set(ref isRunning, value);
    }
    public bool isRunning;
    public bool IsEnabled
    {
      get => isEnabled;
      set => Set(ref isEnabled, value);
    }
    public bool isEnabled = true;
    public ICommand SimulationToggleCommand => new RelayCommand((g) =>
    {
      IsEnabled = false;
      var a = new Timer(1000) { Enabled = true };
      a.Elapsed += (_, __) =>
      {
        IsEnabled = true;
        IsRunning = !IsRunning;
        a.Stop();
        a.Dispose();
      };

    });

    public ObservableCollection<int> SealCountsPercentage
    {
      get => sealCountsPercentage;
    }
    private ObservableCollection<int> sealCountsPercentage
      = new ObservableCollection<int> { 0, 25, 25, 25, 25 };

    public SimulationViewModel()
    {
      InitSliderhandler();
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
        } while ((zeroCheck && percentage == 0) && initial != index);

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
