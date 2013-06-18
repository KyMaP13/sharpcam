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
using Books;


public partial class L1S08 : Window
{


    public L1S08()
    {
        InitializeComponent();
        Books.ItemsSource = CommonBook.Books;
    }
}