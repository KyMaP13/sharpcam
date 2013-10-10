using Model.Tree;

namespace Model.Primitives
{
    public abstract class GcamPoint : NotifierObject
    {
        private double x;
        private double y;

        public double X {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
    }

    public class Frame : SmartItem
    {
        private double x;
        private double y;
        private double angle;

        public Frame (){
            X = 0;
            Y = 0;
            Angle = 0;
        }
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                OnPropertyChanged("Angle");
            }
        }

    }
}
