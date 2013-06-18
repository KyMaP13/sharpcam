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


    public partial class L4S04 : Window
    {
        public L4S04()
        {
            InitializeComponent();
            var binding = new CommandBinding(ApplicationCommands.Cut);
            binding.CanExecute += new CanExecuteRoutedEventHandler(binding_CanExecute);
            binding.Executed += new ExecutedRoutedEventHandler(binding_Executed);
            CommandBindings.Add(binding);
        }

        void binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            list.Items.RemoveAt(list.SelectedIndex);
        }

        void binding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = list.SelectedIndex != -1;
        }
    }
