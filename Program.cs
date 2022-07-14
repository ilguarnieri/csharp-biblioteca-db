//Si vuole progettare un sistema per la gestione di una biblioteca.
//Gli utenti si possono registrare al sistema, fornendo:

//cognome,
//nome,
//email,
//password,
//recapito telefonico,

//I documenti sono caratterizzati da:

//un codice identificativo di tipo stringa (ISBN per i libri, numero seriale per i DVD),
//titolo,
//anno,
//settore(storia, matematica, economia, …),
//stato(In Prestito, Disponibile),
//uno scaffale in cui è posizionato,
//un autore (Nome, Cognome).

//Per i libri si ha in aggiunta il numero di pagine,
//    mentre per i dvd la durata.

//L’utente deve poter eseguire delle ricerche per codice o per titolo e, eventualmente,
//effettuare dei prestiti registrando il periodo (Dal/Al) del prestito e il documento.

//Deve essere possibile effettuare la ricerca dei prestiti dato nome e cognome di un utente.


using csharp_biblioteca;

//creazioni liste
List<User> registeredUsers = new List<User>();
List<Book> books = new List<Book>();
List<Dvd> dvds = new List<Dvd>();
List<Rental> rentals = new List<Rental>();

//utenti registrati
registeredUsers.Add(new User("Angelo", "Guarnieri", "ag@gmail.com", "12345a", "3214567890", true));
registeredUsers.Add(new User("Alessio", "Guarnieri", "al@gmail.com", "12345b", "3224567890", true));
registeredUsers.Add(new User("Roberta", "Verdi", "verdi@gmail.com", "12345c", "3234567890", true));

//articoli della libreria
books.Add(new Book("978-8831007023", "book", "Harry Potter - La serie completa", 2021, "Fantasy", false, "F20", "J.K.Rowling", 3808));
books.Add(new Book("978-8804598909", "book", "Il visconte dimenticato", 2010, "Letteratura", false, "L14", "Italo Calvino", 133));
books.Add(new Book("978-8807900587", "book", "Il ritratto di Dorian Gray", 2013, "Letteratura", false, "L8", "Oscar Wilde", 272));
dvds.Add(new Dvd("B09XR8Q3LY", "dvd", "Animali Fantastici - I Segreti Di Silente", 2022, "Fantasy", false, "F4", "David Yates", 142));
dvds.Add(new Dvd("B09TRK52B4", "dvd", "The Batman", 2022, "Fantasy", false, "F8", "Matt Reeves", 176));
dvds.Add(new Dvd("B09T7CR5CC", "dvd", "Spider-Man No Way Home", 2022, "Fantasy", false, "F6", "Jon Watts", 148));



Library library = new Library(registeredUsers, books, dvds, rentals);

library.Home(" MENÙ");