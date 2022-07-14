using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca
{
    internal class Rental
    {
        public Document document;
        public string startDate;
        public string endDate;
        public int idUser;
        public string name;
        public string surname;
        public string email;

        public Rental(Document document, string startDate, string  endDate, int idUser, string name, string surname, string email)
        {
            this.document = document;
            this.startDate = startDate;
            this.endDate = endDate;
            this.idUser = idUser;
            this.name = name;
            this.surname = surname;
            this.email = email;
        }
    }
}
