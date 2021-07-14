using System;
using System.Globalization;

namespace Book
{
    /// <summary>
    /// Class that represent book instance, and provide the ability to store information about book inside it.
    /// </summary>
    public sealed class Book : IEquatable<Book>, IComparable<Book>, IComparable, IFormattable
    {
        private const int MaxHashLength = 9;

        private bool published;
        private DateTime datePublished;
        private int totalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">The author of the book.</param>
        /// <param name="title">The title of the book.</param>
        /// <param name="publisher">The publisher of the book.</param>
        public Book(string author, string title, string publisher)
            : this(author, title, publisher, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">The author of the book.</param>
        /// <param name="title">The title of the book.</param>
        /// <param name="publisher">The publisher of the book.</param>
        /// <param name="isbn">The ISBN number of the book.</param>
        public Book(string author, string title, string publisher, string isbn)
        {
            this.ISBN = ISBNValidate(isbn) ? isbn : string.Empty;
            this.Title = StringValidate(title);
            this.Publisher = StringValidate(publisher);
            this.Author = StringValidate(author);
        }

        /// <summary>
        /// Gets the author of the book.
        /// </summary>
        /// <value>
        /// The author string value.
        /// </value>
        public string Author { get; }

        /// <summary>
        /// Gets the title of the book.
        /// </summary>
        /// <value>
        /// The title string value.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the publisher of the book.
        /// </summary>
        /// <value>
        /// The publisher string value.
        /// </value>
        public string Publisher { get; }

        /// <summary>
        /// Gets the ISBN number of the book.
        /// </summary>
        /// <value>
        /// The ISBN number string value.
        /// </value>
        public string ISBN { get; }

        /// <summary>
        /// Gets or sets the pages count value.
        /// </summary>
        /// <value>
        /// The pages count integer value.
        /// </value>
        /// <exception cref="ArgumentException">Throws when count of pages less than zero.</exception>
        public int Pages { get => this.totalPages; set => this.totalPages = value >= 0 ? value : throw new ArgumentException("Count of pages can't be negative number"); }

        /// <summary>
        /// Gets the price of the book.
        /// </summary>
        /// <value>
        /// The price decimal value.
        /// </value>
        public decimal Price { get; private set; }

        /// <summary>
        /// Gets the currency symbols of price on the book.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; private set; }

        public static bool operator ==(Book left, Book right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Book left, Book right)
        {
            return !(left == right);
        }

        public static bool operator <(Book left, Book right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Book left, Book right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Book left, Book right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Book left, Book right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Publishes the book and set the date of published.
        /// </summary>
        /// <param name="datePublished">The date of published.</param>
        /// <exception cref="ArgumentException">Throws when date of published greater than now.</exception>
        public void Publish(DateTime datePublished)
        {
            this.published = true;
            this.datePublished = datePublished <= DateTime.Now ? datePublished : throw new ArgumentException("Date of published can't be greater than now.");
        }

        /// <summary>
        /// Gets the publication date of the book.
        /// </summary>
        /// <returns>Date of publication if it's exist, otherwise <c>NYP</c>.</returns>
        public string GetPublicationDate()
        {
            return this.published ? this.datePublished.ToString(CultureInfo.CurrentCulture) : "NYP";
        }

        /// <summary>
        /// Sets the price.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <exception cref="ArgumentException">Price can't be negative number.</exception>
        public void SetPrice(decimal price)
        {
            this.Price = price >= 0 ? price : throw new ArgumentException("Price can't be negative number");
            this.Currency = RegionInfo.CurrentRegion.ISOCurrencySymbol;
        }

        /// <summary>
        /// Converts <see cref="Book"/> class object to string representation.
        /// </summary>
        /// <returns>
        /// String representation of <see cref="Book"/> class.
        /// </returns>
        public override string ToString() => this.ToString("G");

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj switch
        {
            Book book => this.Equals(book),
            _ => false,
        };

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => this.ISBN.GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(Book other) => this.GetHashCode() == other?.GetHashCode() ? this.Title.GetHashCode() == other.Title.GetHashCode() : false;

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero:
        /// this instance precedes <paramref name="other" /> in the sort order.
        /// Zero:
        /// this instance occurs in the same position in the sort order as <paramref name="other" />.
        /// Greater than zero:
        /// this instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(Book other) => other switch
        {
            _ when other is null => 1,
            _ => this.Title.Length - other.Title.Length,
        };

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero:
        /// this instance precedes <paramref name="obj" /> in the sort order.
        /// Zero:
        /// this instance occurs in the same position in the sort order as <paramref name="obj" />.
        /// Greater than zero:
        /// this instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj) => obj switch
        {
            Book book => this.CompareTo(book),
            _ => throw new ArgumentException($"Right hand operand is not {nameof(Book)} instance."),
        };

        private static bool ISBNValidate(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return false;
            }

            if (isbn.Length != 10 && isbn.Length != 13)
            {
                return false;
            }

            return isbn.Length == 13 ? CountISBN13NumberSum(isbn) % 10 == 0 : CountISBN10NumberSum(isbn) % 11 == 0;

            static int CountISBN10NumberSum(string number)
            {
                int numberSum = 0;
                const int XEquivalent = 10;

                for (int index = 0; index < number.Length; index++)
                {
                    if (number[index] == 'X' && index + 1 == number.Length)
                    {
                        numberSum += XEquivalent * (10 - index);
                        continue;
                    }

                    numberSum += int.TryParse(number[index].ToString(), out int digit) ? digit * (10 - index) : throw new ArgumentException("ISBN can't contain any symbols except digits");
                }

                return numberSum;
            }

            static int CountISBN13NumberSum(string number)
            {
                int numberSum = 0;
                int coefficient;
                
                for (int index = 0; index < number.Length; index++)
                {
                    coefficient = 1 + (2 * (index % 2));
                    numberSum += int.TryParse(number[index].ToString(), out int digit) ? digit * coefficient : throw new ArgumentException("ISBN can't contain any symbols except digits");
                }

                return numberSum;
            }
        }

        private static string StringValidate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "String value can't be null or empty");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("String value can't contain only white-spaces");
            }

            return text;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="Book"/> object according to the provided format specifier and current culture format information.
        /// </summary>
        /// <param name="format">The format information.</param>
        /// <returns></returns>
        public string ToString(string format) => this.ToString(format, CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string that represents the <see cref="Book"/> object according to the provided format specifier and culture-specific format information.
        /// </summary>
        /// <param name="format">The format information.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }
            else if(formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            return format.ToUpperInvariant() switch
            {
                "G" => $"{this.Title} by {this.Author}.",
                "C" => $"{this.Title} by {this.Author} {this.Price.ToString("C", formatProvider)}.",
                "DP" =>$"{this} {this.datePublished.Year}. {this.Pages} pages.",
                "DPP" => $"{this} {this.datePublished.Year}. {this.Publisher}. {this.Pages} pages.",
                "DPIP" => $"{this} {this.datePublished.Year}. {this.Publisher}. ISBN: {this.ISBN ?? string.Empty}. {this.Pages} pages.",
                "DPIPC" => $"{this:DPIP} {this.Price.ToString("C", formatProvider)}.",
                _ => throw new FormatException($"The {format} format string is not supported."),
            };
        }

        /// <summary>
        /// Converts the string representation of a number to its Book equivalent.
        /// </summary>
        /// <param name="s">A string that contains a information about book.</param>
        /// <example>Book.Parse("Title",Author,Year,Publisher,Pages,ISBN-13,Price")</example>
        /// <returns>A <see cref="Book"/> object that is equivalent to the information contained in s if source string is valid, otherwise <c>null</c>.</returns>
        public static Book Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the string representation of a number to its Book equivalent according to the current culture format information.
        /// </summary>
        /// <param name="s">A string that contains a information about book.</param>
        /// <param name="result">A <see cref="Book"/> object that is equivalent to the information contained in s if the method returns <c>true</c>, otherwise <c>null</c>.</param>
        /// <example>Book.TryParse("Title",Author,Year,Publisher,Pages,ISBN-13,Price", out result)</example>
        /// <returns><c>true</c> if the parse operation was successful; otherwise, <c>false</c></returns>
        public static bool TryParse(string s, out Book result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>
        /// Converts the string representation of a number to its Book equivalent according to the specified culture format information.
        /// </summary>
        /// <param name="s">A string that contains a information about book.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <example>Book.Parse("Title",Author,Year,Publisher,Pages,ISBN-13,Price", provider)</example>
        /// <returns>A <see cref="Book"/> object that is equivalent to the information contained in s if source string is valid, otherwise <c>null</c>.</returns>
        public static Book Parse(string s, IFormatProvider provider) => TryParse(s, provider, out Book result) ? result : null;

        /// <summary>
        /// Converts the string representation of a number to its Book equivalent according to the specified culture format information.
        /// </summary>
        /// <param name="s">A string that contains a information about book.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="result">A <see cref="Book"/> object that is equivalent to the information contained in s if the method returns <c>true</c>, otherwise <c>null</c>.</param>
        /// <example>Book.TryParse("Title",Author,Year,Publisher,Pages,ISBN-13,Price", provider, out result)</example>
        /// <returns><c>true</c> if the parse operation was successful; otherwise, <c>false</c></returns>
        public static bool TryParse(string s, IFormatProvider provider, out Book result)
        {
            result = null;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var bookData= s.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if(bookData.Length != 7)
            {
                return false;
            }

            string title = bookData[0].Trim('"');
            string author = bookData[1];
            string publisher = bookData[3];
            string isbn = bookData[5].Trim();

            if (!DateTime.TryParse(bookData[2].Trim(), provider, DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowWhiteSpaces, out DateTime dateOfPublishing))
            {
                return false;
            }

            if (!int.TryParse(bookData[4].Trim(), NumberStyles.Integer, provider, out int pages))
            {
                return false;
            }

            if(!ISBNValidate(bookData[5]))
            {
                return false;
            }

            if(!decimal.TryParse(bookData[6].Trim(), NumberStyles.Currency | NumberStyles.AllowDecimalPoint, provider, out decimal price))
            {
                return false;
            }

            try
            {
                result = new Book(author, title, publisher, isbn);

                result.Publish(dateOfPublishing);
                result.Pages = pages;
                result.SetPrice(price);

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
