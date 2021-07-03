using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace ServiceForWorkingWithBooks.Tests.Moq
{
    internal class BookListServiceMoqTests
    {

        private Mock<IStorage<Book.Book>> storageMock;
        private Mock<IBookPredicate> predicateMock;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.storageMock = new Mock<IStorage<Book.Book>>();
            this.predicateMock = new Mock<IBookPredicate>();
        }

        [Test]
        public void BehaviorTest_LoadFromStorageOnce()
        {
            this.storageMock.Setup(storage => storage.Load()).Returns(() => new List<Book.Book>());

            var storage = this.storageMock.Object;
            var service = new BookListService(storage);

            storageMock.Verify(storage => storage.Load(), Times.Once);
        }

        [Test]
        public void StateTest_LoadFromStorageSaveDictionary()
        {
            this.storageMock.Setup(storage => storage.Load()).Returns(() => new List<Book.Book> { new Book.Book("test", "test", "test", "9971502100") });
            predicateMock.Setup(predicate => predicate.Verify(It.IsAny<Book.Book>())).Returns((Book.Book book) => book.ISBN == "9971502100");

            var storage = this.storageMock.Object;
            var predicate = predicateMock.Object;
            var service = new BookListService(storage);

            CollectionAssert.AreEqual(service.FindByTag(predicate), new Book.Book[] {new Book.Book("test", "test", "test", "9971502100") });
        }

        [Test]
        public void BehaviorTest_SaveToStorageOnce()
        {
            this.storageMock.Setup(storage => storage.Load()).Returns(() => new List<Book.Book>());
            this.storageMock.Setup(storage => storage.Save(It.IsAny<IEnumerable<Book.Book>>()));

            var storage = this.storageMock.Object;
            var service = new BookListService(storage);

            service.Save();

            storageMock.Verify(storage => storage.Save(It.IsAny<IEnumerable<Book.Book>>()), Times.Once);
        }

        [Test]
        public void StateTest_AddToLocalDictionaryAndSaveToStorageUpdateStorage()
        {
            // Arrange
            var storage = new FakeBookListStorage();
            var service = new BookListService(storage);
            var book = new Book.Book("test", "test", "test");

            // Act
            service.Add(book);
            service.Save();

            //Assert
            Assert.IsTrue(storage.books.Contains(book));
        }

        [Test]
        public void StateTest_Remove()
        {
            // Arrange
            var storage = new FakeBookListStorage();
            var service = new BookListService(storage);
            var book = new Book.Book("test", "test", "test");

            // Act
            service.Add(book);
            service.Save();

            //Assert
            Assert.IsTrue(storage.books.Contains(book));

            // Act
            service.Remove(book);
            service.Save();

            //Assert
            Assert.IsFalse(storage.books.Contains(book));
        }

        [Test]
        public void StateTest_SortByPrice()
        {
            // Arrange
            var mockComparer = new Mock<IComparer<Book.Book>>();
            mockComparer.Setup(predicate => predicate.Compare(It.IsAny<Book.Book>(), It.IsAny<Book.Book>())).Returns((Book.Book lhs, Book.Book rhs) => (int)(lhs.Price - rhs.Price));
            
            var storage = new FakeBookListStorage();
            var service = new BookListService(storage);
            var comparer = mockComparer.Object;

            var source = new Book.Book[] 
            { 
                new Book.Book("test", "test1", "test", "11111111111"), 
                new Book.Book("test", "test2", "test", "22222222222"), 
                new Book.Book("test", "test3", "test", "33333333333"), 
            };

            var expected = new Book.Book[]
            {
                new Book.Book("test", "test3", "test", "33333333333"),
                new Book.Book("test", "test2", "test", "22222222222"),
                new Book.Book("test", "test1", "test", "11111111111"),
            };

            // Act
            for (int index = 0; index < source.Length; index++)
            {
                source[index].SetPrice(index + 1);
                service.Add(source[index]);
            }

            //Assert
            CollectionAssert.AreEquivalent(expected, service.SortBy(comparer));
        }

        public void StateTest_FindBy()
        {
            predicateMock.Setup(predicate => predicate.Verify(It.IsAny<Book.Book>())).Returns(true);

            var storage = new FakeBookListStorage();
            var service = new BookListService(storage);
            var predicate = predicateMock.Object;

            int countOfBooks = 10;

            for (int count = 0; count < countOfBooks; count++)
            {
                service.Add(new Book.Book("test", "test", "test"));
            }

            var actual = service.FindByTag(predicate);

            Assert.IsTrue(actual.ToArray().Length == countOfBooks);
        }
    }
}
