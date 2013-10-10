namespace Model.Tree
{
    public class Polyline : NotifierObject
    {
        private PointList points;

        public PointList Points {
            get
            {
                return points;
            }
            set
            {
                points = value;
                OnPropertyChanged("Points");
            }
        }
    }

    public abstract class Profile : Polyline
    {
    }
}
