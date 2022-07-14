using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace csharp_biblioteca
{
    internal class Library
    {
        public List<Book> books;
        public List<Rental> rentals;

        private bool isLog = false;
        private int idUser;
        private User userLog;
        private Book bookSearch;

        //db
        string stringaDiConnessione = "Data Source=localhost;Initial Catalog=db-biblioteca;Integrated Security=True";

        //costruttore
        public Library(List<Book> books, List<Rental>rentals)
        {
            this.books = books;
            this.rentals = rentals;
        }



        public void Home(string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine("------------------------------\n");

            Console.WriteLine("1. Registrati");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit\n");

            int choice;

            choice = this.loopChoice(3);


            switch (choice)
            {
                case 1:
                    this.Register();
                    break;
                case 2:
                    this.Login();
                    break;
                case 3:
                    this.Exit();
                    break;
            }
        }



        private void Register()
        {
            Console.Clear();
            Console.WriteLine(" REGISTRAZIONE UTENTE");
            Console.WriteLine("------------------------------\n");

            Console.Write("NOME: ");
            string name = Console.ReadLine();
            Console.Write("COGNOME: ");
            string surname = Console.ReadLine();
            Console.Write("E-MAIL: ");
            string email = Console.ReadLine();
            Console.Write("PASSWORD: ");
            string password = Console.ReadLine();
            Console.Write("TELEFONO: ");
            string cellNumber = Console.ReadLine();


            using(SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    using (SqlTransaction trans = conn.BeginTransaction("UserCreation"))
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandText = "INSERT INTO users (name, surname, email, password, cellNumber, register) VALUES (@name, @surname, @email, @password, @cellNumber, 1)";
                            cmd.Parameters.Add(new SqlParameter("@name", name));
                            cmd.Parameters.Add(new SqlParameter("@surname", surname));
                            cmd.Parameters.Add(new SqlParameter("@email", email));
                            cmd.Parameters.Add(new SqlParameter("@password", password));
                            cmd.Parameters.Add(new SqlParameter("@cellNumber", cellNumber));

                            cmd.ExecuteNonQuery();
                            trans.Commit();

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            Console.Clear();
            this.Home(" REGISTRAZIONE RIUSCITA");
        }



        private void Login()
        {
            Console.Clear();
            Console.WriteLine(" LOGIN");
            Console.WriteLine("------------------------------\n");

            Console.Write("E-MAIL: ");
            string email = Console.ReadLine();
            Console.Write("PASSWORD: ");
            string password = Console.ReadLine();


            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM users WHERE email=@email AND password=@password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@email", email));
                        cmd.Parameters.Add(new SqlParameter("@password", password));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (reader.Read())
                                {
                                    this.isLog = true;

                                    this.idUser = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string surname = reader.GetString(2);
                                    
                                    string cellNumber = reader.GetString(5);
                                    bool register = reader.GetBoolean(6);

                                    this.userLog = new User(name, surname, email, password, cellNumber, register);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (this.isLog)
            {
                Console.Clear();
                this.Logged();                
            }
            else
            {
                this.Home("INSERITI DATI NON CORRETTI");
            }
        }



        private void Logged()
        {
            Console.WriteLine($" BENVENUTO {this.userLog.name.ToUpper()} {this.userLog.surname.ToUpper()}");
            Console.WriteLine("------------------------------\n");

            Console.WriteLine("1. Cerca un libro");
            Console.WriteLine("2. Libri disponibili");
            Console.WriteLine("3. I tuoi noleggi");
            Console.WriteLine("4. Logout");
            Console.WriteLine("5. Exit\n");

            int choice;

            choice = this.loopChoice(5);


            switch (choice)
            {
                case 1:
                    Console.Clear();
                    this.SearchBook();
                    break;
                case 2:
                    this.allBooks();
                    break;
                case 3:
                    this.RentalList(this.idUser);
                    break;
                case 4:
                    this.isLog = false;
                    this.idUser = -1;
                    this.Home(" MENÙ");
                    break;
                case 5:
                    this.Exit();
                    break;
            }
        }


        //---------------------------------------- BOOK ----------------------------------------------------
        private void SearchBook()
        {
            Console.WriteLine(" RICERCA LIBRO");
            Console.WriteLine("------------------------------\n");

            Console.WriteLine("1. Cerca ISBN");
            Console.WriteLine("2. Cerca titolo");
            Console.WriteLine("3. Torna indietro");
            Console.WriteLine("4. Logout");
            Console.WriteLine("5. Exit\n");

            int choice;

            choice = this.loopChoice(5);

            switch (choice)
            {
                case 1:
                    this.SearchBookCode();
                    break;
                case 2:
                    this.SearchBookTitle();
                    break;
                case 3:
                    Console.Clear();
                    this.Logged();
                    break;
                case 4:
                    this.isLog = false;
                    this.idUser = -1;
                    this.Home(" MENÙ");
                    break;
                case 5:
                    this.Exit();
                    break;
            }
        }



        private void SearchBookCode()
        {
            Console.Clear();
            Console.WriteLine(" RICERCA LIBRO");
            Console.WriteLine("------------------------------\n");

            Console.Write($"Inserisci ISBN: ");
            string isbn = Console.ReadLine();

            bool foundBook = false;

            using(SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM books WHERE isbn=@isbn";

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@isbn", isbn));

                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foundBook = true;

                                isbn = reader.GetString(1);
                                string title = reader.GetString(2);
                                string author = reader.GetString(3);
                                int year = reader.GetInt32(4);
                                int numberPages = reader.GetInt32(5);
                                string genre = reader.GetString(6);
                                string shelf = reader.GetString(7);
                                bool state = reader.GetBoolean(8);

                                this.bookSearch = new Book(isbn, "book", title, year, genre, state, shelf, author, numberPages);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (foundBook)
            {
                Console.Clear();
                this.BookInfo(this.bookSearch);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("*- LIBRO NON TROVATO -*\n");
                this.SearchBook();
            }

        }



        private void SearchBookTitle()
        {
            Console.Clear();
            Console.WriteLine(" RICERCA LIBRO");
            Console.WriteLine("------------------------------\n");

            Console.Write($"Inserisci titolo: ");
            string title = Console.ReadLine();


            bool foundBook = false;

            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM books WHERE title=@isbn";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@isbn", title));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foundBook = true;

                                string isbn = reader.GetString(1);
                                title = reader.GetString(2);
                                string author = reader.GetString(3);
                                int year = reader.GetInt32(4);
                                int numberPages = reader.GetInt32(5);
                                string genre = reader.GetString(6);
                                string shelf = reader.GetString(7);
                                bool state = reader.GetBoolean(8);

                                this.bookSearch = new Book(isbn, "book", title, year, genre, state, shelf, author, numberPages);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (foundBook)
            {
                Console.Clear();
                this.BookInfo(this.bookSearch);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("*- LIBRO NON TROVATO -*\n");
                this.SearchBook();
            }
        }



        private void BookInfo(Book book)
        {
            Console.WriteLine($" {book.title}");
            Console.WriteLine("------------------------------\n");

            Console.WriteLine("1. Informazioni");
            Console.WriteLine("2. Noleggia");
            Console.WriteLine("3. Restituisci");
            Console.WriteLine("4. Menù principale");
            Console.WriteLine("5. Logout");
            Console.WriteLine("6. Exit\n");

            int choice;

            choice = this.loopChoice(6);

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    book.stampInfo();
                    Console.WriteLine("\nPremi qualsiasi tasto per tornare indietro...");
                    Console.ReadKey();
                    Console.Clear();
                    this.BookInfo(book);                    
                    break;
                case 2:
                    this.RentalBook(book);
                    break;
                case 3:
                    this.ReturnBook(book);
                    break;
                case 4:
                    Console.Clear();
                    this.Logged();
                    break;
                case 5:
                    this.isLog = false;
                    this.idUser = -1;
                    this.Home(" MENÙ");
                    break;
                case 6:
                    this.Exit();
                    break;
            }
        }



        private void RentalBook(Book book)
        {
            Console.Clear();
            if (!book.state)
            {
                Console.WriteLine($" {book.title}");
                Console.WriteLine("------------------------------\n");

                Console.WriteLine("Noleggia questo libro");
                Console.Write("dal: ");
                string startDate = Console.ReadLine();
                Console.Write("al: ");
                string endDate = Console.ReadLine();

                //creazione prestito
                Rental rent = new Rental(book, startDate, endDate, this.idUser, this.userLog.name, this.userLog.surname, this.userLog.email);
                //aggiunta prestito alla lista dei prestiti
                this.userLog.rentalUser.Add(rent);

                book.state = true;
                Console.Clear();
                Console.WriteLine("Operazione eseguita con successo!");
                Console.WriteLine($"Hai noleggiato {book.title} di {book.author}");
                Console.WriteLine($"Ricordati di restituirlo entro il {endDate}");
                Console.WriteLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~\n");
                this.Logged();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Mi dispiace il libro non è disponibile.\n");
                this.BookInfo(book);
            }
        }



        private void ReturnBook(Book book)
        {
            Console.Clear();

            int choice;
            bool bookPresent = false;
            int keyRent = -1;

            Console.WriteLine($" {book.title}");
            Console.WriteLine("------------------------------\n");

            for (int i = 0; i < this.userLog.rentalUser.Count; i++)
            {
                if (this.userLog.rentalUser[i].document.id == book.id)
                {
                    bookPresent = true;
                    keyRent = i;
                    break;
                }
            }

            if (bookPresent)
            {
                Console.WriteLine("Vuoi restituire questo libro?");
                Console.WriteLine("1. SI");
                Console.WriteLine("2. NO\n");

                choice = this.loopChoice(2);

                switch (choice)
                {
                    case 1:
                        this.userLog.rentalUser.RemoveAt(keyRent);
                        book.state = false;
                        Console.Clear();
                        Console.WriteLine($"{book.title} restituito con successo!");
                        Console.WriteLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~\n");
                        this.Logged();
                        break;
                    case 2:
                        Console.Clear();
                        this.BookInfo(book);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Non hai noleggiato questo libro.\n");
                Console.WriteLine("1. Torna alle info");
                Console.WriteLine("2. Menù principale");
                Console.WriteLine("3. Logout");
                Console.WriteLine("4. Exit\n");

                choice = this.loopChoice(4);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        this.BookInfo(book);
                        break;
                    case 2:
                        Console.Clear();
                        this.Logged();
                        break;
                    case 3:
                        this.isLog = false;
                        this.idUser = -1;
                        this.Home(" MENÙ");
                        break;
                    case 4:
                        this.Exit();
                        break;
                }
            }
        }



        //---------------------------------------- RENTAL ----------------------------------------------------
        private void RentalList(int idUser)
        {
            Console.Clear();

            Console.WriteLine(" I TUOI NOLEGGI");
            Console.WriteLine("------------------------------\n");


            if(this.userLog.rentalUser.Count < 1)
            {
                Console.WriteLine("Non c'è nessun noleggio in corso\n\n");
                Console.WriteLine("Premi qualsiasi tasto per tornare indietro...");
                Console.ReadKey();
                Console.Clear();
                this.Logged();
            }
            else
            {
                string stamp = this.userLog.rentalUser.Count > 1 ?
                    $"Ci sono {this.userLog.rentalUser.Count} noleggi in corso:" :
                    $"C'è {this.userLog.rentalUser.Count} noleggio in corso:\n";

                Console.WriteLine(stamp);

                for (int i = 0; i < this.userLog.rentalUser.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    Console.WriteLine($"{this.userLog.rentalUser[i].document.title} ");
                    Console.Write($"   noleggio dal {this.userLog.rentalUser[i].startDate} ");
                    Console.Write($"al {this.userLog.rentalUser[i].endDate}\n\n");
                }

                Console.WriteLine("Seleziona un articolo");

                int choice;

                choice = this.loopChoice(this.userLog.rentalUser.Count);

                if (this.userLog.rentalUser[choice - 1].document.type == "book")
                {
                    foreach (Book book in books)
                    {
                        if (book.id == this.userLog.rentalUser[choice - 1].document.id)
                        {
                            Console.Clear();
                            this.BookInfo(book);
                            break;
                        }
                    }
                }
                else
                {
                    
                }
            }
        }


        private void Exit()
        {
            Console.Clear();
            Console.WriteLine(" TORNA A TROVARCI PRESTO!");
            Console.WriteLine("------------------------------\n");
        }






        //---------------------------------------- ALL LIST ----------------------------------------------------
        public void allBooks()
        {
            Console.Clear();
            Console.WriteLine(" LISTA LIBRI DISPONIBILI");
            Console.WriteLine("------------------------------\n");

            foreach(Book book in books)
            {
                if (!book.state)
                {
                    Console.WriteLine($"- {book.title} di {book.author}, {book.year}, {book.id}");
                }
            }


            Console.WriteLine("\nPremi qualsiasi tasto per tornare indietro...");
            Console.ReadKey();
            Console.Clear();
            this.Logged();
        }




        //---------------------------------------- CONTROLLO INPUT MENU ----------------------------------------------------
        private int loopChoice(int options)
        {
            int choice;
            do
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out choice);

                if (String.IsNullOrEmpty(input) || !isNumber)
                {
                    input = "0";
                }

                choice = Convert.ToByte(input);

                if (choice == 0 || choice > options)
                {
                    Console.WriteLine("La voce di menu selezionata non esite!");
                    Console.WriteLine("Inserisci una voce valida...\n");
                }
            } while (choice == 0 || choice > options);

            return choice;
        }
    }
}
