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


public partial class L1S02 : Window
{
    public class Book
    {
        public string Authors { get; set; }
        public string Title { get; set; }
        public bool InStock { get; set; }
    }

    Book book;

    public L1S02()
    {
        InitializeComponent();
        book = new Book();
        book.Authors = "George Martin";
        book.Title = "Game of Thrones";
        book.InStock = true;
        this.DataContext = book;
    }

    void CmOk(object sender, RoutedEventArgs args)
    {
        MessageBox.Show(string.Format("{0}\n{1}\n{2}", book.Authors, book.Title, book.InStock));
    }

    void DeleteFromStock(object sender, RoutedEventArgs args)
    {
        book.InStock = false;
    }
    
}
