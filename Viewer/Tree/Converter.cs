using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Viewer.Tree
{
    // <summary>
    // A converter that organizes several collections into (optional)
    // child collections that are put into <see cref="FolderItem"/>
    // containers.
    // </summary>

    public class Converter : IMultiValueConverter
    {
        // <summary>
        // A converter that organizes several collections into (optional)
        // child collections that are put into <see cref="FolderItem"/>
        // </summary>

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //get folder name listing...
            var folder = parameter as string ?? String.Empty;
            var folders = folder.Split(',').Select(f => f.Trim()).ToList();
            //...and make sure there are no missing entries
            while (values.Length > folders.Count) folders.Add(String.Empty);

            //this is the collection that gets all top level items
            var items = new List<object>();

            for (int i = 0; i < values.Length; i++)
            {
                //make sure were working with collections from here...
                var childs = values[i] as IEnumerable ?? new List<object> { values[i] };

                string folderName = folders[i];
                if (folderName != String.Empty)
                {
                    //create folder item and assign childs
                    var folderItem = new ContainerNode { Name = folderName, Items = childs };
                    items.Add(folderItem);
                }
                else
                {
                    //if no folder name was specified, move the item directly to the root item
                    items.AddRange(childs.Cast<object>());
                }
            }

            return items;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot perform reverse-conversion");
        }
    }
}
