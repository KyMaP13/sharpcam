using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Pocket : Shape
    {
        //private String _name = "Bolt Hole";
        private double _height;
        private double _length;
        private double _width;
        private double _Y;
        public Pocket()
        {
            this.Name = "Pocket";
        }
        public double height
        {
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
        public double length
        {
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
        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                OnPropertyChanged("Y");
            }
        }
        public double width
        {
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
    }



}