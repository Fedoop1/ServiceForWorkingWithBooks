using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceForWorkingWithBooks.Tests.Moq
{
    public class FakeBookListStorage : IStorage<Book.Book>
    {
        public IEnumerable<Book.Book> books;
        public FakeBookListStorage(IEnumerable<Book.Book> books) => this.books = books;

        public FakeBookListStorage() { this.books = new List<Book.Book>(); }

        public IEnumerable<Book.Book> Load() => this.books;

        public void Save(IEnumerable<Book.Book> data) => this.books = data;
    }
}
