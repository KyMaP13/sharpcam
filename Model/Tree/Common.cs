using System;
using System.Collections.ObjectModel;

namespace Model.Tree
{
    // <summary>
    // Class implementing events for deleting
    // and moving items. Items shuld be derived from <see cref="SmartItem" />.
    // </summary>

    public class SmartList<T>: ObservableCollection<T> where T: SmartItem, new()
    {
        private void AddItem(object sender, EventArgs e)
        {
            var item = (T)sender;
            var index = IndexOf(item);
            InsertItem(index, new T());
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            var item = (T)sender;
            Remove(item);
            
        }

        private void MoveItemUp(object sender, EventArgs e)
        {
            var item = (T)sender;
            var index = IndexOf(item);
            if (index <= 0) return;
            RemoveAt(index);
            index--;
            base.InsertItem(index, item);
        }

        private void MoveItemDown(object sender, EventArgs e)
        {
            var item = (T)sender;
            var index = IndexOf(item);
            if (index >= Count - 1) return;
            RemoveAt(index);
            index++;
            base.InsertItem(index, item);
        }

        public new void Add(T item)
        {
            base.Add(item);
            item.AddHandler += AddItem;
            item.DeleteHandler += DeleteItem;
            item.MoveUpHandler += MoveItemUp;
            item.MoveDownHandler += MoveItemDown;
        }
    }

    // <summary>
    // Items for <see cref="SmartList" />.
    // Implements delete and move events.
    // </summary>

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
