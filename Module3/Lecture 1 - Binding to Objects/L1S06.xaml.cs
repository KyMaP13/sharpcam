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


public partial class L1S06 : Window
{
    public class Book
    {
        static Regex ISBNPattern = new Regex("^[0-9-]*$");
        string authors;
        public string Authors
        {
            get { return authors; }
            set 
            {
                if (value == "") throw new ArgumentException("Имя автора не должно быть пустым");
                authors = value; 
            }
        }
        string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (value == "") throw new ArgumentException("Название книги не должно быть пустым");
                title = value;
            }
        }
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

    public L1S06()
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
      
        if (!IsValid(this))
        {
            MessageBox.Show("При заполнении формы допущены ошибки", "L1S06", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        MessageBox.Show(string.Format("{0}\n{1}\n{2}", book.Authors, book.Title, book.InStock));
    }

    private bool IsValid(DependencyObject obj)
    {
        return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(child => IsValid(child));
    }
    
}
