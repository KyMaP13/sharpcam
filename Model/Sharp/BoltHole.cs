using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BoltHole : Shape
    {
        private double _Radius;
        private double _length;
        private double _lenAll;
        private double _Y;
        private double _radius;

        public BoltHole()
        {
            this.Name = "Bolt Hole";
        }
        public double Radius
        {
            get
            {
                return _Radius;
            }
            set
            {
                _Radius = value;
                //this.modified = true;
                OnPropertyChanged("Radius");
            }
        }
        public double lenAll
        {
            get
            {
                return _lenAll;
            }
            set
            {
                _lenAll = value;
                //this.modified = true;
                OnPropertyChanged("LengthAll");
            }
        }
        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
               // this.modified = true;
                OnPropertyChanged("Y");
            }
        }
        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                //this.modified = true;
                OnPropertyChanged("Length");
            }
        }
        public double radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                //this.modified = true;
                OnPropertyChanged("radius");
            }
        }
        
    }



}

