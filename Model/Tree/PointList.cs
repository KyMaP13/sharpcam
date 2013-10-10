using System.Collections.ObjectModel;
using Model.Primitives;

namespace Model.Tree
{
    public abstract class PointList : ObservableCollection<GcamPoint>
    {
        // <summary>
        // Перегрузка этого метода для представления узла в дереве
        // </summary>

        public override string ToString()
        {
            return "Points:";
        }
    }
}