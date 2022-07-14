using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Rental
    {
        public int IdBook { get; private set; }
        public int IdUser { get; private set; }
        public string title;
        public string author;
        public string startDate;
        public string endDate;

        public Rental(int idBook, string title, string author, string startDate, string endDate, int idUser)
        {
            this.IdBook = idBook;
            this.title = title;
            this.author = author;
            this.startDate = startDate;
            this.endDate = endDate;
            this.IdUser = idUser;
        }
    }
}
