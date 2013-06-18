using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Books
{
    class CommonBook
    {

        public string Title { get; set; }
        public string Author { get; set; }
        public bool InStock { get; set; }
        public string Publisher { get; set; }
        public string Comment { get; set; }
        public override string ToString()
        {
            return Title;
        }

        public static IEnumerable<CommonBook> Books
        {
            get
            {
                yield return new CommonBook { Title = "The Eye of the World", Author = "Robets Jordan", InStock = true, Publisher = "TOR", Comment = "" };
                yield return new CommonBook { Title = "The Game of Thrones", Author = "George Martin", InStock = false, Publisher = "Bantam Spectra", Comment = "" };
                yield return new CommonBook { Title = "Pro C# and .NET", Author = "Andrew Troelsen", InStock = true, Publisher = "Apress", Comment = "" };
            }
        }
    }
}