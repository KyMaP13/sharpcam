using Model.Tree;

namespace Model.Primitives
{
    public class BoltHole : Shape
    {
        private double externalRadius;
        private double length;
        private double lenAll;
        private double y;
        private double internalRadius;

        public BoltHole()
        {
            Name = "Bolt Hole";
        }
        public double Radius
        {
            get
            {
                return externalRadius;
            }
            set
            {
                externalRadius = value;
                //this.modified = true;
                OnPropertyChanged("Radius");
            }
        }
        public double LenAll
        {
            get
            {
                return lenAll;
            }
            set
            {
                lenAll = value;
                //this.modified = true;
                OnPropertyChanged("LengthAll");
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
               // this.modified = true;
                OnPropertyChanged("Y");
            }
        }
        public double Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
                //this.modified = true;
                OnPropertyChanged("Length");
            }
        }
        public double InternalRadius
        {
            get
            {
                return internalRadius;
            }
            set
            {
                internalRadius = value;
                //this.modified = true;
                OnPropertyChanged("InternalRadius");
            }
        }
        
    }



}

