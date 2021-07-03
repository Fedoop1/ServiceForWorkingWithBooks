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
        private readonly IStorage<Book.Book> bookStorage;
        private HashSet<Book.Book> bookSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListService"/> class.
        /// </summary>
        /// <param name="storage">The storage source.</param>
        /// <exception cref="ArgumentNullException">Throws when storage is null.</exception>
        public BookListService(IStorage<Book.Book> storage)
        {
            this.bookStorage = storage ?? throw new ArgumentNullException(nameof(storage), "Storage is null");
            this.bookSet = LoadStorageFromSequence(this.bookStorage.Load());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListService"/> class.
        /// </summary>
        public BookListService()
        {
            this.bookStorage = new BookListStorage();
            this.bookSet = LoadStorageFromSequence(this.bookStorage.Load());
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

            if (this.bookSet.Contains(book))
            {
                throw new ArgumentException($"{book} is already exists.");
            }

            this.bookSet.Add(book);
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

            if (!this.bookSet.Contains(book))
            {
                throw new ArgumentException($"{book} doesn't exist.");
            }

            this.bookSet.Remove(book);
        }

        /// <summary>
        /// Finds the books which consider by special tag and return it's.
        /// </summary>
        /// <param name="bookPredicate">The book predicate.</param>
        /// <returns>Sequence of books which match predicate.</returns>
        /// <exception cref="ArgumentNullException">Throws when predicate is null.</exception>
        public IReadOnlyCollection<Book.Book> FindByTag(IBookPredicate bookPredicate)
        {
            if (bookPredicate is null)
            {
                throw new ArgumentNullException(nameof(bookPredicate), "Book predicate is null");
            }

            var result = new List<Book.Book>();

            foreach (var book in this.bookSet)
            {
                if (bookPredicate.Verify(book))
                {
                    result.Add(book);
                }
            }

            return result;
        }

        /// <summary>
        /// Sorts the book storage by special comparer and return it's.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Sorted sequence of books.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when comparer is null.</exception>
        public IReadOnlyCollection<Book.Book> SortBy(IComparer<Book.Book> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer), "Book comparer is null");
            }

            var result = this.bookSet.ToArray();

            Array.Sort(result, comparer);

            return result;
        }

        /// <summary>
        /// Loads books from inner storage.
        /// </summary>
        /// <exception cref="NullReferenceException">Throws when storage is null.</exception>
        public void Load() => this.bookSet = LoadStorageFromSequence(this.bookStorage.Load()) ?? throw new NullReferenceException("Storage is null");

        /// <summary>
        /// Saves books from local to inner storage.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when book in local storage already exist in inner storage.</exception>
        public void Save() => this.bookStorage.Save(this.bookSet);

        private static HashSet<Book.Book> LoadStorageFromSequence(IEnumerable<Book.Book> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source), "Source of books is null");
            }

            var result = new HashSet<Book.Book>();
            
            foreach (var book in source)
            {
                result.Add(book);
            }

            return result;
        }
    }
}
