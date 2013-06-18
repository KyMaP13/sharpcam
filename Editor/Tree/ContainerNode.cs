using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Editor
{
    public class ContainerNode : Model.NotifierObject
    {
        private String name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value+" (container)";
                OnPropertyChanged("Name"); //!!!
            }
        }

        private IEnumerable items;
        public IEnumerable Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public override string ToString()
        {
            return "Container: " + name + " / " + items.ToString();
        }
    }
}
