using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Rental
    {
        public string title;
        public string author;
        public DateTime startDate;
        public DateTime endDate;
        public int idUser;

        public Rental(string title, string author, DateTime startDate, DateTime endDate, int idUser)
        {
            this.title = title;
            this.author = author;
            this.startDate = startDate;
            this.endDate = endDate;
            this.idUser = idUser;
        }
    }
}
