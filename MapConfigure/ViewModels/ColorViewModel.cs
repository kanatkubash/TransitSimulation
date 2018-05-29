namespace MapConfigure.ViewModels
{
  using System.Windows.Media;

  public class ColorViewModel : ViewModelBase
  {
    public Color? Color
    {
      get => color;
      set => Set(ref color, value);
    }
    private Color? color;
    public string ColorText
    {
      get => colorText;
      set => Set(ref colorText, value);
    }
    private string colorText;
  }
}
