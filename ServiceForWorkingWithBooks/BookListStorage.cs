using System;
using System.Collections.Generic;

namespace ServiceForWorkingWithBooks
{
    /// <summary>
    /// Class "storage" which contain some condition of <see cref="BookListService"/> instance.
    /// </summary>
    public class BookListStorage : IStorage<IDictionary<string, Book.Book>>
    {
        private IDictionary<string, Book.Book> books;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListStorage"/> class.
        /// </summary>
        public BookListStorage() => this.books = new Dictionary<string, Book.Book>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListStorage"/> class.
        /// </summary>
        /// <param name="books">The books.</param>
        /// <exception cref="ArgumentNullException">Throws when source of storage is null.</exception>
        public BookListStorage(IDictionary<string, Book.Book> books) => this.books = books ?? throw new ArgumentNullException(nameof(books), "Source of book storage is null");

        /// <inheritdoc/>
        public IDictionary<string, Book.Book> Load() => this.books;

        /// <inheritdoc/>
        public void Save(IDictionary<string, Book.Book> data)
        {
            foreach (var item in data ?? throw new ArgumentNullException(nameof(data), "List of books is null"))
            {
                if (this.books.ContainsKey(item.Key))
                {
                    throw new ArgumentException(nameof(item), $"{item} is already exists.");
                }
            }

            this.books = data;
        }
    }
}
