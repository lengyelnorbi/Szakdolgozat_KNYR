﻿using LiveCharts.Wpf;
using LiveCharts;
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
using Szakdolgozat.ViewModels;
using LiveCharts.Defaults;
using System.Runtime.Serialization;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for UjDiagrammokView.xaml
    /// </summary>
    public partial class UjDiagrammokView : UserControl
    {
        public UjDiagrammokView()
        {
            InitializeComponent();
        }

        //private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        //{
        //    var r = new Random();

        //    foreach (var series in TestDoghnutSeriesForShow)
        //    {
        //        foreach (var observable in series.Values.Cast<ObservableValue>())
        //        {
        //            observable.Value = r.Next(0, 10);
        //        }
        //    }
        //}

        //private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        //{
        //    var r = new Random();
        //    var c = TestDoghnutSeriesForShow.Count > 0 ? TestDoghnutSeriesForShow[0].Values.Count : 5;

        //    var vals = new ChartValues<ObservableValue>();

        //    for (var i = 0; i < c; i++)
        //    {
        //        vals.Add(new ObservableValue(r.Next(0, 10)));
        //    }

        //    TestDoghnutSeriesForShow.Add(new PieSeries
        //    {
        //        Values = vals
        //    });
        //}

        //private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        //{
        //    if (TestDoghnutSeriesForShow.Count > 0)
        //        TestDoghnutSeriesForShow.RemoveAt(0);
        //}

        //private void AddValueOnClick(object sender, RoutedEventArgs e)
        //{
        //    var r = new Random();
        //    foreach (var series in TestDoghnutSeriesForShow)
        //    {
        //        series.Values.Add(new ObservableValue(r.Next(0, 10)));
        //    }
        //}

        //private void RemoveValueOnClick(object sender, RoutedEventArgs e)
        //{
        //    foreach (var series in TestDoghnutSeriesForShow)
        //    {
        //        if (series.Values.Count > 0)
        //            series.Values.RemoveAt(0);
        //    }
        //}

        //private void RestartOnClick(object sender, RoutedEventArgs e)
        //{
        //    Chart.Update(true, true);
        //}
    }
}
