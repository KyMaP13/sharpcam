namespace Model.Tree
{
    public class ProjectSettings : NotifierObject
    {
        private double length;
        private double height;
        private double width;
        private double tooldiam;
        private double safedist;
        private double feedrate;

        public double Length{
            get
            {
                return length;
            }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }
        public double Height {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        public double Width {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }
        public double ToolDiam
        {
            get
            {
                return tooldiam;
            }
            set
            {
                tooldiam = value;
                OnPropertyChanged("Tool diam");
            }
        }
        public double SafeDistance
        {
            get
            {
                return safedist;
            }
            set
            {
                safedist = value;
                OnPropertyChanged("Safe distance");
            }
        }
        public double FeedRate
        {
            get
            {
                return feedrate;
            }
            set
            {
                feedrate = value;
                OnPropertyChanged("Feed rate");
            }
        }
    }

    // <summary>
    // ObservableCollection для автоматического обновления содержимого дерева
    // </summary>
   
    public class OperationsList : SmartList<Operation>
    {
        // <summary>
        // Перегрузка этого метода для представления узла в дереве
        // </summary>

        public override string ToString()
        {
            return "Operations";
           
        }

    }

    public class Project : NotifierObject
    {
        private OperationsList operations;
        private ProjectSettings settings;

        public OperationsList Operations {
            get
            {
                return operations;
            }
            set
            {
                operations = value;
                OnPropertyChanged("Operations");
            }
        }

        public ProjectSettings Settings {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                OnPropertyChanged("Settings");
            }
        }
    }
}
