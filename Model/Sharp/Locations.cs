using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    
    public interface ILocationSource
    {
        ObservableCollection<Frame> LocationsList { get; }
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

        private new void deleteItem(object sender, EventArgs e)
        {
            if (this.Count > 1)
            {
                base.deleteItem(sender, e);
            }
        }
    }
    public class CustomLocations : NotifierObject, ILocationSource
    {
        private FrameList _locations;
        public FrameList Locations {
            get
            {
                return _locations;
            }
            set
            {
                _locations = value;
                OnPropertyChanged("Locations");
                OnPropertyChanged("LocationsList");
            }
        }
        
        ObservableCollection<Frame> ILocationSource.LocationsList
        {
            get {
                return this._locations;
            }
        }
        
    }

    public class MatrixLocations : NotifierObject, ILocationSource
    {

        private double _xPadding;
        private double _yPadding;
        private int _rowCount;
        private int _columnCount;

        public double XPadding
        {
            get
            {
                return _xPadding;
            }
            set
            {
                 _xPadding = value;
                OnPropertyChanged("XPadding");
            }
        }
        public double YPadding {get
            {
                return _yPadding;
            }
            set
            {
                 _yPadding = value;
                OnPropertyChanged("YPadding");
            }}
        public int RowCount {get
            {
                return _rowCount;
            }
            set
            {
                 _rowCount = value;
                OnPropertyChanged("RowCount");
            }}
        public int ColumnCount {get
            {
                return _columnCount;
            }
            set
            {
                 _columnCount = value;
                OnPropertyChanged("ColumnCount");
            }}
        

        public ObservableCollection<Frame> LocationsList
        {
            get { throw new NotImplementedException(); }
        }
        
    }
}
