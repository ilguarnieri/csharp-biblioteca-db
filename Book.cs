using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Book : Document
    {
        public string isbn;
        public int numberPages;

        //costruttore
        public Book(int id, string isbn, string type, string title, int year, string sector, bool state, string shelf, string author, int numberPages) :
            base(id, type, title, year, sector, state, shelf, author)
        {
            this.isbn = isbn;
            this.numberPages = numberPages;
        }


        public override void stampInfo()
        {
            base.stampInfo();
            Console.WriteLine($"Numero pagine: {this.numberPages}");
        }

    }
}
