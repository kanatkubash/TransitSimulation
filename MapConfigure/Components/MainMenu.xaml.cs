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
    //public static readonly DependencyProperty WindowProperty = DependencyProperty.Register(
    //  "Window", typeof(MainWindow), typeof(MainMenu), new PropertyMetadata(default(MainWindow)));

    //public MainWindow Window
    //{
    //  get => (MainWindow)GetValue(WindowProperty);
    //  set => SetValue(WindowProperty, value);
    //}

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
  }
}
