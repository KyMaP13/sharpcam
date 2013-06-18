using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace Model
{
    public class PointList : ObservableCollection<GcamPoint>
    {
        /// <summary>
        /// Перегрузка этого метода для представления узла в дереве
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Points:";
        }
    }

    public class Polyline : NotifierObject
    {
        private PointList _points;

        public PointList Points {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                OnPropertyChanged("Points");
            }
        }

        public IEnumerable<IGCamPrimitive> GetGcamPolyline()
        {
            for (int i=0;i<Points.Count;i++)
            {
                yield return new GCamLine() { X0 = Points[i].X, Y0 = Points[i].Y, X1 = Points[i + 1].X, Y1 = Points[i + 1].Y };
            }
        }
    }

    public class Profile : Polyline
    {
    }

    public class Conture : Polyline
    {
        private bool _isClosed;

        public bool IsClosed {
            get
            {
                return _isClosed;
            }
            set
            {
                _isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }
    }
}
