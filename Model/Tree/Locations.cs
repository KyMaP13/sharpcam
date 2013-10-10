using System;
using System.Collections.Generic;
using Model.Primitives;

namespace Model.Tree
{
    
    public interface ILocationSource
    {
        IEnumerable<Frame> LocationsList { get; }
    }
    
    public class FrameList : SmartList<Frame>
    {
        /// <summary>
        /// Перегрузка этого метода для представления узла в дереве
        /// </summary>
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            return "Frames:";
        }
    }
    public class CustomLocations : NotifierObject, ILocationSource
    {
        private FrameList locations;
        public FrameList Locations {
            set
            {
                locations = value;
                OnPropertyChanged("Locations");
                OnPropertyChanged("LocationsList");
            }
        }
        
        IEnumerable<Frame> ILocationSource.LocationsList{get {return locations;}}    
    }

    public abstract class MatrixLocations : NotifierObject, ILocationSource
    {

        private double xPadding;
        private double yPadding;
        private int rowCount;
        private int columnCount;

        public double XPadding
        {
            get
            {
                return xPadding;
            }
            set
            {
                 xPadding = value;
                OnPropertyChanged("XPadding");
            }
        }
        public double YPadding {get
            {
                return yPadding;
            }
            set
            {
                 yPadding = value;
                OnPropertyChanged("YPadding");
            }}
        public int RowCount {get
            {
                return rowCount;
            }
            set
            {
                 rowCount = value;
                OnPropertyChanged("RowCount");
            }}
        public int ColumnCount {get
            {
                return columnCount;
            }
            set
            {
                 columnCount = value;
                OnPropertyChanged("ColumnCount");
            }}
        

        public IEnumerable<Frame> LocationsList
        {
            get { throw new NotImplementedException(); }
        }
        
    }
}
