using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceForWorkingWithBooks.Tests.Moq
{
    public class FakeBookListStorage : IStorage<IDictionary<string, Book.Book>>
    {
        public FakeBookListStorage(Dictionary<string, Book.Book> books) => this.books = books;

        public FakeBookListStorage() { }

        public IDictionary<string, Book.Book> books = new Dictionary<string, Book.Book>();

        public IDictionary<string, Book.Book> Load() => this.books;

        public void Save(IDictionary<string, Book.Book> data) => this.books = data;
    }
}
