﻿using System.Windows.Data;

namespace MapConfigure.Components
{
  using System;
  using System.Collections.Generic;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Media;

  public class DataGridBehavior
  {
    #region DisplayRowNumber

    public static DependencyProperty DisplayRowNumberProperty =
      DependencyProperty.RegisterAttached("DisplayRowNumber",
        typeof(bool),
        typeof(DataGridBehavior),
        new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
    public static bool GetDisplayRowNumber(DependencyObject target) => (bool)target.GetValue(DisplayRowNumberProperty);

    public static void SetDisplayRowNumber(DependencyObject target, bool value)
    {
      target.SetValue(DisplayRowNumberProperty, value);
    }

    private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      var dataGrid = target as DataGrid;
      if ((bool)e.NewValue == true)
      {
        EventHandler<DataGridRowEventArgs> loadedRowHandler = null;
        loadedRowHandler = (object sender, DataGridRowEventArgs ea) =>
        {
          if (GetDisplayRowNumber(dataGrid) == false)
          {
            dataGrid.LoadingRow -= loadedRowHandler;
            return;
          }

          ea.Row.Header = ea.Row.DataContext == CollectionView.NewItemPlaceholder ? "*" : (ea.Row.GetIndex() + 1).ToString();
        };
        dataGrid.LoadingRow += loadedRowHandler;

        ItemsChangedEventHandler itemsChangedHandler = null;
        itemsChangedHandler = (object sender, ItemsChangedEventArgs ea) =>
        {
          if (GetDisplayRowNumber(dataGrid) == false)
          {
            dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
            return;
          }

          GetVisualChildCollection<DataGridRow>(dataGrid).ForEach(d => d.Header = d.GetIndex());
        };
        dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
      }
    }

    #endregion // DisplayRowNumber

    #region Get Visuals

    private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
    {
      var visualCollection = new List<T>();
      GetVisualChildCollection(parent as DependencyObject, visualCollection);
      return visualCollection;
    }

    private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
    {
      var count = VisualTreeHelper.GetChildrenCount(parent);
      for (var i = 0; i < count; i++)
      {
        var child = VisualTreeHelper.GetChild(parent, i);
        if (child is T)
          visualCollection.Add(child as T);
        if (child != null)
          GetVisualChildCollection(child, visualCollection);
      }
    }

    #endregion // Get Visuals
  }
}
