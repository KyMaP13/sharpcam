using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectSettings : NotifierObject
    {
        private double _length;
        private double _height;
        private double _width;
        private double _tooldiam;
        private double _safedist;
        private double _feedrate;

        public double Length{
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged("Length");
            }
        }
        public double Height {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
        public double Width {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        public double ToolDiam
        {
            get
            {
                return _tooldiam;
            }
            set
            {
                _tooldiam = value;
                OnPropertyChanged("Tool diam");
            }
        }
        public double SafeDistance
        {
            get
            {
                return _safedist;
            }
            set
            {
                _safedist = value;
                OnPropertyChanged("Safe distance");
            }
        }
        public double FeedRate
        {
            get
            {
                return _feedrate;
            }
            set
            {
                _feedrate = value;
                OnPropertyChanged("Feed rate");
            }
        }
    }

    /// <summary>
    /// ObservableCollection для автоматического обновления содержимого дерева
    /// </summary>
    public class OperationsList : SmartList<Operation>
    {
        /// <summary>
        /// Перегрузка этого метода для представления узла в дереве
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Operations";
           
        }

    }

    public class Project : NotifierObject
    {
        private OperationsList _operations;
        private ProjectSettings _settings;

        public OperationsList Operations {
            get
            {
                return _operations;
            }
            set
            {
                _operations = value;
                OnPropertyChanged("Operations");
            }
        }

        public ProjectSettings Settings {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                OnPropertyChanged("Settings");
            }
        }
    }
}
