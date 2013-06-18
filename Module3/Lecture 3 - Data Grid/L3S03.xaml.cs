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
using System.Collections.ObjectModel;
using Books;


public partial class L3S03 : Window
{
    ObservableCollection<CommonBook> books;
    public L3S03()
    {
        InitializeComponent();
        books = new ObservableCollection<CommonBook>();
        foreach (var e in CommonBook.Books)
            books.Add(e);
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
    }
}