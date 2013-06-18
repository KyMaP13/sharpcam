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


public partial class L3S02 : Window
{
    List<CommonBook> books;
    public L3S02()
    {
        InitializeComponent();
        books = CommonBook.Books.ToList();
        grid.ItemsSource = books;
    }

    void CmOk(object sender, RoutedEventArgs args)
    {
        var res = "";
        foreach (var e in books)
            res += e.Author + "\n";
        MessageBox.Show(res);
    }

    void AddBook(object sender, RoutedEventArgs args)
    {
        books.Add(new CommonBook
        {
            Title = "Fifth Elephant",
            Author = "Terry Pratchett"
        });
        grid.Items.Refresh();
    }
}