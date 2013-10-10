using Model.Tree;

namespace Model.Primitives
{
    public class Pocket : Shape
    {
        private double height;
        private double length;
        private double width;
        private double y;
        public Pocket()
        {
            Name = "Pocket";
        }
        public double Height
        {
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
        public double Length
        {
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
        public double Width
        {
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
    }



}