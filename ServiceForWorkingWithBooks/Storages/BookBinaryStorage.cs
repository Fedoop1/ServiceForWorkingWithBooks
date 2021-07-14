using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceForWorkingWithBooks.Storages
{
    /// <summary>
    /// Class "storage" that load and save book information from file stream.
    /// </summary>
    /// <seealso cref="IStorage&lt;Book.Book&gt;" />
    public class BookBinaryStorage : IStorage<Book.Book>
    {
        // "Title" - int, Author - string, Year - DateTime(int.int.int), Publisher - string, Pages - int, ISBN-13 - string, Price - decimal 
        private const int BookBufferSize = sizeof(int) + sizeof(decimal) + sizeof(int) + sizeof(int) + sizeof(int) + (4 * StringSize);

        private const int StringSize = 120;
        private readonly FileStream destinationFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookBinaryStorage"/> class.
        /// </summary>
        /// <param name="path">The path to storage file.</param>
        /// <exception cref="ArgumentException">Throws when file path is null or empty or whitespace.</exception>
        public BookBinaryStorage(string path)
        {
            InputValidation(path);
            this.destinationFile = new FileStream(path, FileMode.Create);
        }

        ~BookBinaryStorage()
        {
            this.destinationFile.Close();
        }

        /// <inheritdoc/>
        public IEnumerable<Book.Book> Load()
        {
            using var binaryReader = new BinaryReader(this.destinationFile, Encoding.UTF8, true);
            binaryReader.BaseStream.Position = 0;
            var buffer = new byte[BookBufferSize];
            var bookSet = new HashSet<Book.Book>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                buffer = binaryReader.ReadBytes(BookBufferSize);
                var book = FromBinaryToBookConverter(buffer);
                bookSet.Add(book);
            }

            return bookSet;
        }

        /// <inheritdoc/>
        public void Save(IEnumerable<Book.Book> data)
        {
            using var binaryWriter = new BinaryWriter(this.destinationFile, Encoding.UTF8, true);
            var buffer = new byte[BookBufferSize];

            foreach (var book in data ?? throw new ArgumentNullException(nameof(data), "List of books is null"))
            {
                buffer = BookToByteConverter(book);
                binaryWriter.Write(buffer);
            }
        }

        private static byte[] BookToByteConverter(Book.Book book)
        {
            var buffer = new byte[BookBufferSize];
            using var memoryStream = new MemoryStream(buffer);
            using var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(book.Title);
            binaryWriter.Write(book.Author);

            if (book.GetPublicationDate() == "NYP")
            {
                binaryWriter.Write(0);
                binaryWriter.Write(0);
                binaryWriter.Write(0);
            }
            else
            {
                DateTime dateOfPublishing = DateTime.Parse(book.GetPublicationDate(), CultureInfo.CurrentCulture);

                binaryWriter.Write(dateOfPublishing.Month);
                binaryWriter.Write(dateOfPublishing.Day);
                binaryWriter.Write(dateOfPublishing.Year);
            }

            binaryWriter.Write(book.Publisher);
            binaryWriter.Write(book.Pages);
            binaryWriter.Write(book.ISBN);
            binaryWriter.Write(book.Price);

            return buffer;
        }

        private static Book.Book FromBinaryToBookConverter(byte[] buffer)
        {
            using var memoryStream = new MemoryStream(buffer);
            using var binaryReader = new BinaryReader(memoryStream);

            var title = binaryReader.ReadString();
            var author = binaryReader.ReadString();
            var month = binaryReader.ReadInt32();
            var day = binaryReader.ReadInt32();
            var year = binaryReader.ReadInt32();
            var publisher = binaryReader.ReadString();
            var pages = binaryReader.ReadInt32();
            var isbn = binaryReader.ReadString();
            var price = binaryReader.ReadDecimal();

            var result = new Book.Book(author, title, publisher, isbn);

            if ((day, month, year) != (0, 0, 0))
            {
                result.Publish(new DateTime(year, month, day));
            }

            result.Pages = pages;
            result.SetPrice(price);

            return result;
        }

        private static void InputValidation(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentException($"{nameof(sourcePath)} cannot be null or empty or whitespace.", nameof(sourcePath));
            }
        }
    }
}
