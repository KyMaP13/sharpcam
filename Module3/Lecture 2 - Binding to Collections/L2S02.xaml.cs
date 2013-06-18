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


public partial class L1S09 : Window
{
    List<CommonBook> books;
    public L1S09()
    {
        InitializeComponent();
        books = CommonBook.Books.ToList();
        bookList.ItemsSource = books;
    }

    void AddBook(object sender, RoutedEventArgs args)
    {
        books.Add(new CommonBook
        {
            Author = "Robert Jordan, Brandon Sanderson",
            Title = "The Memory of Light",
            Publisher = "TOR",
            InStock = false,
            Comment = ""
        });
        bookList.Items.Refresh();
    }
}