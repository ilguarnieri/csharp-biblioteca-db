using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Document
    {
        public int id;
        public string type;
        public string title;
        public int year;
        public string sector;
        public bool state;
        public string shelf;
        public string author;

        //costruttore
        public Document(int id, string type, string title, int year, string sector, bool state, string shelf, string author)
        {
            this.id = id;
            this.type = type;
            this.title = title;
            this.year = year;
            this.sector = sector;
            this.state = state;
            this.shelf = shelf;
            this.author = author;
        }


        public virtual void stampInfo()
        {
            Console.WriteLine($" {this.title}");
            Console.WriteLine($" by {this.author}");
            Console.WriteLine("------------------------------\n");

            Console.WriteLine($"Anno {this.year}");
            Console.WriteLine($"Genere: {this.sector}");
            Console.WriteLine($"Scaffale: {this.shelf}");
            Console.Write("Disponibile: ");
            if (!this.state)
            {
                Console.WriteLine("SI");
            }
            else
            {
                Console.WriteLine("NO");
            }

        }
    }
}
