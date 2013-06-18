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


public partial class L4S01 : Window
{
    public L4S01()
    {
        InitializeComponent();
        this.MouseDown += Handler;
        label.MouseDown += Handler;
        ellipse.MouseDown += Handler;
    }

    void Handler(object sender, RoutedEventArgs args)
    {
        trace.Items.Add(sender.GetType().Name);
    }
//!args.Handled=true|false
}
