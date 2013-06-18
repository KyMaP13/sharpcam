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
using System.Text.RegularExpressions;


public partial class L1S04 : Window
{
    public class Book
    {
        static Regex ISBNPattern = new Regex("^[0-9-]*$");
        public string Authors { get; set; }
        public string Title { get; set; }
        public bool InStock { get; set; }
        public string isbn;
        public string ISBN 
        { 
            get { return isbn; }
            set 
            {
                if (!ISBNPattern.Match(value).Success)
                    throw new ArgumentException("ISBN должен состоять из цифр и дефисов");
                isbn=value;
            }
        }
    }

   

    Book book;

    public L1S04()
    {
        InitializeComponent();
        book = new Book();
        book.Authors = "George Martin";
        book.Title = "Game of Thrones";
        book.InStock = true;
        book.ISBN="123-456-789";
        this.DataContext = book;
    }

    void CmOk(object sender, RoutedEventArgs args)
    {
        MessageBox.Show(string.Format("{0}\n{1}\n{2}", book.Authors, book.Title, book.InStock));
    }
}
