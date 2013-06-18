using System.Windows;

using System.Collections.ObjectModel;
using Model;
using Model.Samples;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Project testProject = Samples.DummyProject;
        private ObservableCollection<Project> source = new ObservableCollection<Project>();

        public MainWindow()
        {
            InitializeComponent();
            OperationsTree.ItemsSource = source;
            source.Add(Samples.DummyProject);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            source[0].Settings.Height = 42;
        }

        private void TestButton2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(source[0].Settings.Height.ToString());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            source[0] = Samples.DummyProject;
        }
    }
}
