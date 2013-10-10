using System;

namespace Model.Tree
{
    // <summary>
    // Форма: описывает отверстие, вырез, и т.д.
    // </summary>
    
    public class Shape : NotifierObject
    {
        private String name = "Shape";
        // <summary>
        // Имя для отображения в дереве
        // </summary>
        
        public String Name {
            get
            {
                return name;
            }
            set //позволяем задавать имя унаследованным классам
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
