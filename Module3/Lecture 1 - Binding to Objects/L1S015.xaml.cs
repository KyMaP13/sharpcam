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


public partial class L1S015 : Window
{
    public class Publisher
    {
        public string Name { get; set; }
        public string URL { get; set; }
    }

    public class Book
    {
        public string Authors { get; set; }
        public string Title { get; set; }
        public Publisher Publisher { get; set; }
    }

    Book book;

    public L1S015()
    {
        InitializeComponent();
        book = new Book
        {
            Authors = "Robert Jordan",
            Title = "The Eye of The World",
            Publisher = new Publisher
            {
                Name = "TOR",
                URL = "tor.com"
            }
        };
        this.DataContext = book;
    }

    
}
