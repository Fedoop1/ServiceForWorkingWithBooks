using System;
using NUnit.Framework;

namespace ServiceForWorkingWithBooks.Tests
{
    [TestFixture]
    internal class BookListServiceTests
    {
        [Test]
        public void Add_NullBook_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new BookListService().Add(null), "Book is null");

        [Test]
        public void SortBy_NullComparer_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new BookListService().SortBy(null), "Book comparer is null");

        [Test]
        public void FindBy_NullPredicate_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new BookListService().FindByTag((IBookPredicate)null), "Book predicate is null");

        [Test]
        public void Remove_NullBook_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new BookListService().Remove(null), "Book is null");

        [Test]
        public void Remove_BookDoesntExist_ThrowArgumentException() => Assert.Throws<ArgumentException>(() => new BookListService().Remove(new Book.Book("test", "test", "test")), "Book is null");

        [Test]
        public void Ctor_NullStorage_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new BookListService(null), "Storage is null");

        [Test]
        public void Add_ExistsBook_ThrowArgumentException()
        {
            var service = new BookListService();
            var book = new Book.Book("test", "test", "test", "9971502100");

            service.Add(book);

            Assert.Throws<ArgumentException>(() => service.Add(book), $"{book} is already exists.");
        }
    }
}
