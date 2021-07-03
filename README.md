# ServiceForWorkingWithBooks
Develop a Book class. (AutoCode .NET Development (2020Q4 / BY / 3) Group 1. The Book Class (no skeleton, no tests))
Implement equivalence and order relations for objects of the class. (AutoCode .NET Development (2020Q4 / BY / 3) Group 1. The Book Class. Comparisons (no skeleton, no tests))
To work with a list of books, create a service class BookListService with the functionality
Add (add a book if there is no such book, otherwise throw an exception);
Remove (remove the book if it exists, otherwise throw an exception);
FindByTag (find books by a given criterion);
SortBy (sort the list of books by a given criterion);
Load (load a list of books from the BookListStorage).
Save (save the list of books to some storage BookListStorage).
When implementing methods, do not use delegates and LINQ!
Implement unit and mock tests. To test the interaction of the Load and Save methods of the service class with the storage, 
use FakeBookListStorage as a wrapper for the list of books (the List <Book> class), which is stored in memory ("in memory dataset"). 
FakeBookListStorage is used for testing purposes only, further storage will be changed.
