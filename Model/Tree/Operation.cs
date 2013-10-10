namespace Model.Tree
{


    public class Operation : SmartItem
    {
        private Shape shape;
        private ILocationSource location;

        public Shape Shape {
            get
            {
                return shape;
            }
            set
            {
                 shape = value;
                OnPropertyChanged("Shape");
            }
        }
        
        public ILocationSource Location {
            get
            {
                return location;
            }
            set
            {
                 location = value;
                OnPropertyChanged("Location");
            }
        }
        
    }
}
