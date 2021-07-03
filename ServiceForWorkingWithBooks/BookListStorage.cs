using System;
using System.Collections.Generic;

namespace ServiceForWorkingWithBooks
{
    /// <summary>
    /// Class "storage" which contain some condition of <see cref="BookListService"/> instance.
    /// </summary>
    public class BookListStorage : IStorage<Book.Book>
    {
        private HashSet<Book.Book> books;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListStorage"/> class.
        /// </summary>
        public BookListStorage() => this.books = new HashSet<Book.Book>();

        /// <inheritdoc/>
        public IEnumerable<Book.Book> Load() => this.books;

        /// <inheritdoc/>
        public void Save(IEnumerable<Book.Book> data)
        {
            foreach (var item in data ?? throw new ArgumentNullException(nameof(data), "List of books is null"))
            {
                if (this.books.Contains(item))
                {
                    throw new ArgumentException(nameof(item), $"{item} is already exists.");
                }
            }

            this.books = (HashSet<Book.Book>)data;
        }
    }
}
