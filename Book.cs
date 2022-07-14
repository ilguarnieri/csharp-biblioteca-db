using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Book : Document
    {
        public string Isbn
        {
            get
            {
                return base.id;
            }
            set
            {
                base.id = value;
            }
        }

        public int numberPages;

        //costruttore
        public Book(string isbn, string type, string title, uint year, string sector, bool state, string shelf, string author, int numberPages) :
            base(isbn, type, title, year, sector, state, shelf, author)
        {
            this.numberPages = numberPages;
        }


        public override void stampInfo()
        {
            base.stampInfo();
            Console.WriteLine($"Numero pagine: {this.numberPages}");
        }

    }
}
