using System;
using System.Windows;
public class Runner
{
    [STAThread]
    public static void Main()
    {
        new Application().Run(new L4S04());
    }
}