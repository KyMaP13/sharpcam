using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


public partial class L4S02 : Window
{
    public L4S02()
    {
        InitializeComponent();
        this.PreviewMouseDown+= Handler;
        label.PreviewMouseDown += Handler;
        ellipse.PreviewMouseDown += Handler;
    }

    void Handler(object sender, RoutedEventArgs args)
    {
        trace.Items.Add(sender.GetType().Name);
    }
//!args.Handled=true|false
}
