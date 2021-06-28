using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceForWorkingWithBooks
{
    /// <summary>
    /// Class which manage list of books and store them's in inner storage.
    /// </summary>
    public class BookListService
    {
        private readonly IStorage<IDictionary<string, Book.Book>> bookStorage;
        private IDictionary<string, Book.Book> bookDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListService"/> class.
        /// </summary>
        /// <param name="storage">The storage source.</param>
        /// <exception cref="System.ArgumentNullException">Throws when storage is null.</exception>
        public BookListService(IStorage<IDictionary<string, Book.Book>> storage)
        {
            this.bookStorage = storage ?? throw new ArgumentNullException(nameof(storage), "Storage is null");
            this.bookDictionary = this.bookStorage.Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListService"/> class.
        /// </summary>
        public BookListService()
        {
            this.bookStorage = new BookListStorage();
            this.bookDictionary = this.bookStorage.Load();
        }

        /// <summary>
        /// Adds the book to the book storage.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <exception cref="ArgumentNullException">Throws when book is null.</exception>
        /// <exception cref="ArgumentException">Throws when book is already exist.</exception>
        public void Add(Book.Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book), "Book is null");
            }

            if (this.bookDictionary.ContainsKey(book.ISBN))
            {
                throw new ArgumentException($"{book} is already exists.");
            }

            this.bookDictionary.Add(book.ISBN, book);
        }

        /// <summary>
        /// Removes the book from the book storage.
        /// </summary>
        /// <param name="book">The book to remove.</param>
        /// <exception cref="ArgumentNullException">Throws when book is null.</exception>
        /// <exception cref="ArgumentException">Throws when book doesn't exist.</exception>
        public void Remove(Book.Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book), "Book is null");
            }

            if (!this.bookDictionary.ContainsKey(book.ISBN))
            {
                throw new ArgumentException($"{book} doesn't exist.");
            }

            this.bookDictionary.Remove(book.ISBN);
        }

        /// <summary>
        /// Finds the books which consider by special tag and return it's.
        /// </summary>
        /// <param name="bookPredicate">The book predicate.</param>
        /// <returns>Sequence of books which match predicate.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when predicate is null.</exception>
        public IEnumerable<Book.Book> FindByTag(IBookPredicate bookPredicate)
        {
            if (bookPredicate is null)
            {
                throw new ArgumentNullException(nameof(bookPredicate), "Book predicate is null");
            }

            return FindBy(bookPredicate);

            IEnumerable<Book.Book> FindBy(IBookPredicate bookPredicate)
            {
                foreach (var book in this.bookDictionary.Values)
                {
                    if (bookPredicate.Verify(book))
                    {
                        yield return book;
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the book storage by special comparer and return it's.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Sorted sequence of books.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when comparer is null.</exception>
        public IEnumerable<Book.Book> SortBy(IComparer<Book.Book> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer), "Book comparer is null");
            }

            return Sort(comparer);

            IEnumerable<Book.Book> Sort(IComparer<Book.Book> comparer)
            {
                var array = this.bookDictionary.Values.ToArray();
                Array.Sort(array, comparer);

                foreach (var book in array)
                {
                    yield return book;
                }
            }
        }

        /// <summary>
        /// Loads books from inner storage.
        /// </summary>
        /// <exception cref="System.NullReferenceException">Throws when storage is null.</exception>
        public void Load() => this.bookDictionary = this.bookStorage.Load() ?? throw new NullReferenceException("Storage is null");

        /// <summary>
        /// Saves books from local to inner storage.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when book in local storage already exist in inner storage.</exception>
        public void Save() => this.bookStorage.Save(this.bookDictionary);
    }
}
