namespace MapConfigure.ViewModels
{
  using System.ComponentModel;
  using System.Runtime.CompilerServices;

  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public T Set<T>(ref T prevVal, T newVal, [CallerMemberName] string propName = "")
    {
      if (prevVal != null && prevVal.Equals(newVal))
        return prevVal;
      if (prevVal == null && newVal == null)
        return prevVal;
      prevVal = newVal;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

      return newVal;
    }
  }
}
