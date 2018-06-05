namespace MapConfigure.Components
{
  using System.Windows;
  using System.Windows.Controls;
  using ViewModels;

  /// <summary>
  /// Interaction logic for MainMenu.xaml
  /// </summary>
  public partial class MainMenu : UserControl
  {
    public MainWindow Window
    {
      get => window;
      set
      {
        window = value;
        window.StdOutTextBox = StdOutTextBox;
      }
    }
    private MainWindow window;

    public MainMenu()
    {
      InitializeComponent();
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
      StdOutTextBox.Clear();
    }
  }
}
