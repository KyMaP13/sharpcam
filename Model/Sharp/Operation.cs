using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{


    public class Operation : SmartItem
    {
        private Shape _shape;
        private ILocationSource _location;

        public Shape Shape {
            get
            {
                return _shape;
            }
            set
            {
                 _shape = value;
                OnPropertyChanged("Shape");
            }
        }
        
        public ILocationSource Location {
            get
            {
                return _location;
            }
            set
            {
                 _location = value;
                OnPropertyChanged("Location");
            }
        }
        
    }
}
