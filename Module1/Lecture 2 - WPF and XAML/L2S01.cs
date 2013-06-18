using System;

using System.Windows;
using System.Windows.Controls;

class L2S01
{
    [STAThread]
    public static void MainX()
    {
        var label = new Label();
        label.Content = "Hello, WPF!";
        label.Width=100;
        label.Height=30;

        var button = new Button();
        button.Click += button_Click;
        button.Content = "OK";
        button.Width=50;
        button.Height=30;

        var canvas = new Canvas();
        canvas.Children.Add(label);
        canvas.Children.Add(button);
        Canvas.SetTop(button, 40);
        Canvas.SetLeft(button, 35);

        var wnd = new Window();
        wnd.Content = canvas;
        wnd.Width= 140;
        wnd.Height = 140;
       
        var app = new Application();
        app.Run(wnd);
    }

    static void button_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Hello WPF again");
    }
}
