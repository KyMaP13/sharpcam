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
using System.Collections;


public partial class L3S06 : Window
{
    public L3S06()
    {
        InitializeComponent();
        var books = this.FindResource("books") as ArrayList;
        books.Clear();
        books.AddRange(CommonBook.Books.ToList());
    }
}