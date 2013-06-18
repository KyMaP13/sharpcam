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


public partial class L3S04 : Window
{
    List<CommonBook> books;
    public L3S04()
    {
        InitializeComponent();

        grid.Columns[2].SetValue(
            DataGridComboBoxColumn.ItemsSourceProperty,
            CommonBook.Books.Select(z => z.Publisher).Distinct().ToList());
        
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
}