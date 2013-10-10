using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// Class implementing events for deleting
    /// and moving items. Items shuld be derived from <see cref="SmartItem" />.
    /// </summary>
    public class SmartList<T>: ObservableCollection<T>
        where T: SmartItem, new()
    {
        protected void addItem(object sender, EventArgs e)
        {
            T item = (T)sender;
            int index = base.IndexOf(item);
            this.InsertItem(index, new T());
        }
        protected void deleteItem(object sender, EventArgs e)
        {
            T item = (T)sender;
            base.Remove(item);
            
        }
        protected void moveItemUp(object sender, EventArgs e)
        {
            T item = (T)sender;
            int index = base.IndexOf(item);
            if (index > 0)
            {
                base.RemoveAt(index);
                index--;
                base.InsertItem(index, item);
            }
        }

        protected void moveItemDown(object sender, EventArgs e)
        {
            T item = (T)sender;
            int index = base.IndexOf(item);
            if (index < base.Count-1)
            {
                base.RemoveAt(index);
                index++;
                base.InsertItem(index, item);
            }
        }

        public new void Add(T item)
        {
            base.Add(item);
            item.AddHandler += this.addItem;
            item.DeleteHandler += this.deleteItem;
            item.MoveUpHandler += this.moveItemUp;
            item.MoveDownHandler += this.moveItemDown;
        }
    }

    /// <summary>
    /// Items for <see cref="SmartList" />.
    /// Implements delete and move events.
    /// </summary>
    public class SmartItem : NotifierObject
    {
        public event EventHandler AddHandler;
        public event EventHandler DeleteHandler;
        public event EventHandler MoveUpHandler;
        public event EventHandler MoveDownHandler;
        public void AddEvent()
        {
            if (AddHandler != null)
            {
                AddHandler(this, EventArgs.Empty);
            }
            else
            {
                throw new InvalidOperationException("Add handler is empty.");
            }
        }
        public void DeleteEvent()
        {
            if (DeleteHandler != null)
            {
                DeleteHandler(this, EventArgs.Empty);
            }
            else
            {
                throw new InvalidOperationException("Delete handler is empty.");
            }
        }
        public void MoveUpEvent()
        {
            if (MoveUpHandler != null)
            {
                MoveUpHandler(this, EventArgs.Empty);
            }
            else
            {
                throw new InvalidOperationException("MoveUp handler is empty.");
            }
        }
        public void MoveDownEvent()
        {
            if (MoveDownHandler != null)
            {
                MoveDownHandler(this, EventArgs.Empty);
            }
            else
            {
                throw new InvalidOperationException("MoveDown handler is empty.");
            }
        }

    }
}
