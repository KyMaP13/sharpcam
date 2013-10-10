using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GcamPoint : NotifierObject
    {
        private double _x;
        private double _y;

        public double X {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }
    }

    public class Frame : SmartItem
    {
        private double _x;
        private double _y;
        private double _angle;

        public Frame (){
            this.X = 0;
            this.Y = 0;
            this.Angle = 0;
        }
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                OnPropertyChanged("Angle");
            }
        }

    }
}
