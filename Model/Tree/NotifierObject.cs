using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Model
{
    /// <summary>
    /// Simple base class that provides a solid implementation
    /// of the <see cref="INotifyPropertyChanged"/> event.
    /// </summary>
    public abstract class NotifierObject : INotifyPropertyChanged
    {
        private bool _modified = true;
        ///<summary>
        ///Occurs when a property value changes.
        ///</summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for
        /// a given property.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                this._modified = true;
            }
        }
        public bool modified
        {
            get
            {
                return _modified;
            }
        }
        public void isDrawn()
        {
            this._modified = false;
        }
    }
}
