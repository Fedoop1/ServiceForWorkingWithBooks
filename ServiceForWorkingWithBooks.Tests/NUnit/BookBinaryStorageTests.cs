using System.Collections.Generic;
using NUnit.Framework;
using ServiceForWorkingWithBooks.Storages;

namespace ServiceForWorkingWithBooks.Tests.NUnit
{
    [TestFixture]
    internal class BookBinaryStorageTests
    {
        [TestCaseSource(typeof(BookSource), nameof(BookSource.BooksForStorages))]
        public void SaveAndLoad_ValidData_ReturnSequnceOfBooks(IEnumerable<Book.Book> booksSource)
        {
            const string path = "bookTestBinaryStorage.db";
            var binaryStorage = new BookBinaryStorage(path);

            var bookService = new BookListService(binaryStorage);

            foreach (var book in booksSource)
            {
                bookService.Add(book);
            }

            bookService.Save();

            bookService.Load();

            var serviceBooks = bookService.FindByTag(x => x != null);

            CollectionAssert.AreEqual(booksSource, serviceBooks);
        }
    }
}
