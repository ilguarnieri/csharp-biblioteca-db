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
    

        private bool isLog = false;
        private User userLog;
        private Book bookSelect;

        //db
        string stringaDiConnessione = "Data Source=localhost;Initial Catalog=db-biblioteca;Integrated Security=True";

        //costruttore
        public Library()
        {
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
                    conn.Close();
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

                                    int idUser = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string surname = reader.GetString(2);
                                    
                                    string cellNumber = reader.GetString(5);
                                    bool register = reader.GetBoolean(6);

                                    this.userLog = new User(idUser, name, surname, email, password, cellNumber, register);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());                                
                            }
                        }
                    }
                    conn.Close();
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
                    this.RentalList();
                    break;
                case 4:
                    this.isLog = false;
                    this.userLog = null;
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
                    this.userLog = null;
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

                                int id = reader.GetInt32(0);
                                isbn = reader.GetString(1);
                                string title = reader.GetString(2);
                                string author = reader.GetString(3);
                                int year = reader.GetInt32(4);
                                int numberPages = reader.GetInt32(5);
                                string genre = reader.GetString(6);
                                string shelf = reader.GetString(7);
                                bool state = reader.GetBoolean(8);

                                this.bookSelect = new Book(id, isbn, "book", title, year, genre, state, shelf, author, numberPages);

                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (foundBook)
            {
                Console.Clear();
                this.BookInfo(this.bookSelect);
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

                                int id = reader.GetInt32(0);
                                string isbn = reader.GetString(1);
                                title = reader.GetString(2);
                                string author = reader.GetString(3);
                                int year = reader.GetInt32(4);
                                int numberPages = reader.GetInt32(5);
                                string genre = reader.GetString(6);
                                string shelf = reader.GetString(7);
                                bool state = reader.GetBoolean(8);

                                this.bookSelect = new Book(id, isbn, "book", title, year, genre, state, shelf, author, numberPages);

                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (foundBook)
            {
                Console.Clear();
                this.BookInfo(this.bookSelect);
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
                    this.userLog = null;
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
                Console.Write("fino al (dd/mm/yyyy): ");
                DateTime endDate = Convert.ToDateTime(Console.ReadLine());


                using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        using (SqlTransaction trans = conn.BeginTransaction("RentalCreation"))
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = trans;

                            try
                            {
                                cmd.CommandText = "INSERT INTO rentals (book_id, user_id, start_date, end_date, state) VALUES (@book_id, @user_id, @start_date, @end_date, 1)";
                                cmd.Parameters.Add(new SqlParameter("@book_id", book.id));
                                cmd.Parameters.Add(new SqlParameter("@user_id", this.userLog.Id));
                                cmd.Parameters.Add(new SqlParameter("@start_date", DateTime.Now));
                                cmd.Parameters.Add(new SqlParameter("@end_date", endDate));
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = "UPDATE books SET state=1 WHERE id=@idBook";
                                cmd.Parameters.Add(new SqlParameter("@idBook", book.id));
                                cmd.ExecuteNonQuery();

                                trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                            }
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

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


            //controllo se il libro è stato noleggiato dal cliente
            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM rentals WHERE user_id=@user_id AND book_id=@book_id AND state=1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@user_id", this.userLog.Id));
                        cmd.Parameters.Add(new SqlParameter("@book_id", book.id));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookPresent = true;
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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
                        using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
                        {
                            try
                            {
                                conn.Open();

                                string query = "UPDATE rentals SET state=0 WHERE user_id=@user_id AND book_id=@book_id";

                                SqlCommand cmd = new SqlCommand(query, conn);
                                cmd.Parameters.Add(new SqlParameter("@user_id", this.userLog.Id));
                                cmd.Parameters.Add(new SqlParameter("@book_id", book.id));

                                cmd.ExecuteNonQuery();

                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
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
                        this.userLog = null;
                        this.Home(" MENÙ");
                        break;
                    case 4:
                        this.Exit();
                        break;
                }
            }
        }



        //---------------------------------------- RENTAL ----------------------------------------------------
        private void RentalList()
        {
            Console.Clear();

            Console.WriteLine(" I TUOI NOLEGGI");
            Console.WriteLine("------------------------------\n");

            List<Rental> rentals = new List<Rental>();

            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    using (SqlTransaction trans = conn.BeginTransaction("UserCreation"))
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        cmd.CommandText = "SELECT * FROM rentals WHERE user_id=@user_id AND state=1";
                        cmd.Parameters.Add(new SqlParameter("@user_id", this.userLog.Id));
                        cmd.ExecuteNonQuery();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int book_id = reader.GetInt32(1);
                                DateTime startDate = reader.GetDateTime(3);
                                DateTime endDate = reader.GetDateTime(4);

                                string title = "";
                                string author = "";

                                cmd.CommandText = "SELECT title, author FROM book WHERE book_id=@book_id";
                                cmd.Parameters.Add(new SqlParameter("@book_id", book_id));
                                cmd.ExecuteNonQuery();

                                using (SqlDataReader reader2 = cmd.ExecuteReader())
                                {
                                    if (reader2.Read())
                                    {
                                        title = reader2.GetString(0);
                                        author = reader2.GetString(1);
                                    }
                                }

                                Rental rental = new Rental(title, author, startDate, endDate, this.userLog.Id);
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }


























            if (rentals.Count < 1)
            {
                Console.WriteLine("Non c'è nessun noleggio in corso\n\n");
                Console.WriteLine("Premi qualsiasi tasto per tornare indietro...");
                Console.ReadKey();
                Console.Clear();
                this.Logged();
            }
            else
            {
                string stamp = rentals.Count > 1 ?
                    $"Ci sono {rentals.Count} noleggi in corso:" :
                    $"C'è {rentals.Count} noleggio in corso:\n";

                Console.WriteLine(stamp);

                for (int i = 0; i < rentals.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    Console.WriteLine($"{rentals[i].title} ");
                    Console.Write($"   noleggio dal {rentals[i].startDate} ");
                    Console.Write($"al {rentals[i].endDate}\n\n");
                }

                Console.WriteLine("\nPremi qualsiasi tasto per tornare indietro...");
                Console.ReadKey();
                Console.Clear();
                this.Logged();
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

            Book bookDB;

            List<Book> books = new List<Book>();

            using (SqlConnection conn = new SqlConnection(stringaDiConnessione))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM books WHERE state=0";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {

                                int id = reader.GetInt32(0);
                                string isbn = reader.GetString(1);
                                string title = reader.GetString(2);
                                string author = reader.GetString(3);
                                int year = reader.GetInt32(4);
                                int numberPages = reader.GetInt32(5);
                                string genre = reader.GetString(6);
                                string shelf = reader.GetString(7);
                                bool state = reader.GetBoolean(8);

                                bookDB = new Book(id, isbn, "book", title, year, genre, state, shelf, author, numberPages);

                                books.Add(bookDB);
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            foreach (Book book in books)
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
