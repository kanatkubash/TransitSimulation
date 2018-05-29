using System;
using System.Collections.Generic;
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
  using Core.Entities;
  using ViewModels;

  /// <summary>
  /// Interaction logic for MapNodeShape.xaml
  /// </summary>
  public partial class MapNodeShape : UserControl
  {
    private MapNodeVm MapNodeVm;

    public MapNodeShape(MapNodeVm mapNodeVm)
    {
      InitializeComponent();
      MapNodeVm = mapNodeVm;
      DataContext = MapNodeVm;
    }
  }
}
