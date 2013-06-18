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
using System.Diagnostics.Contracts;


namespace Utils
{
    public class StringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringList = (List<string>)value;
            return stringList.Aggregate((a, b) => a + parameter.ToString() + b);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value as string).Split(new string[] { parameter.ToString() }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}

public partial class L1S07 : Window
{
    class Book
    {
        public List<string> Authors { get; set; }
        public string Title { get; set; }
    }

    Book book;

    public L1S07()
    {
        InitializeComponent();
        book = new Book
        {
            Authors = new List<string> { "Robert Jordan", "Brandon Sanderson" },
            Title = "The towers of Midnight"
        };
        DataContext = book;
    }

    public void CmOk(object sender, RoutedEventArgs args)
    {
        MessageBox.Show(book.Authors.Count.ToString());
    }
}
