using MapConfigure.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapConfigure.Components
{
  /// <summary>
  /// Interaction logic for CButton.xaml
  /// </summary>
  public partial class CButton : UserControl
  {
    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }
    public object CommandParameter
    {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
    }

    public static readonly DependencyProperty CommandProperty =
      DependencyProperty.Register("Command", typeof(ICommand), typeof(CButton));
    public static readonly DependencyProperty CommandParameterProperty =
      DependencyProperty.Register("CommandParameter", typeof(object), typeof(CButton));
    public static readonly DependencyProperty BackProperty =
      DependencyProperty.Register("Back", typeof(Brush), typeof(CButton));
    public static readonly DependencyProperty KindProperty =
      DependencyProperty.Register("Kind", typeof(PackIconKind), typeof(CButton));

    public Brush Back
    {
      get => (Brush)GetValue(BackProperty);
      set => SetValue(BackProperty, value);
    }
    public PackIconKind Kind
    {
      get => (PackIconKind)GetValue(KindProperty);
      set => SetValue(KindProperty, value);
    }

    public CButton()
    {
      InitializeComponent();
    }
  }
}
