using System;
using System.Collections;
using Model.Tree;

namespace Viewer.Tree
{
    public class ContainerNode : NotifierObject
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
                name = value;// +" (container)";
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
            return "Container: " + name + " / " + items;
        }
    }
}
