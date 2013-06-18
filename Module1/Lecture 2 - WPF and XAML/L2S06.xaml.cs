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


public partial class L2S06 : Window
{
    public L2S06()
    {
        InitializeComponent();
    }

    private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
    {
        rectangle.Fill=new SolidColorBrush(Colors.Green);
    }
}
