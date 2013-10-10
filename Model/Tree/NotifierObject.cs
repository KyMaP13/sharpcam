using System.ComponentModel;

namespace Model.Tree
{
    // <summary>
    // Simple base class that provides a solid implementation
    // of the <see cref="INotifyPropertyChanged"/> event.
    // </summary>

    public abstract class NotifierObject : INotifyPropertyChanged
    {
        private bool modified = true;

        //<summary>
        //Occurs when a property value changes.
        //</summary>

        public virtual event PropertyChangedEventHandler PropertyChanged;

        // <summary>
        // Raises the <see cref="PropertyChanged"/> event for
        // a given property.
        // </summary>
        // <param name="propertyName">The name of the changed property.</param>

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                modified = true;
            }
        }
        public bool Modified
        {
            get
            {
                return modified;
            }
        }
        public void IsDrawn()
        {
            modified = false;
        }
    }
}
