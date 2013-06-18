using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// Форма: описывает отверстие, вырез, и т.д.
    /// </summary>
    public class Shape : NotifierObject
    {
        private String _name = "Shape";
        /// <summary>
        /// Имя для отображения в дереве
        /// </summary>
        public String Name {
            get
            {
                return _name;
            }
            set //позволяем задавать имя унаследованным классам
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
