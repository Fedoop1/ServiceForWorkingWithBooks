using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceForWorkingWithBooks
{
    /// <summary>
    /// Interface which provides the ability to find concrete records in <see cref="BookListService"/> by the implementing this interface in specific classes.
    /// </summary>
    public interface IBookPredicate
    {
        /// <summary>
        /// Verifies if the specified book match the predicate.
        /// </summary>
        /// <param name="book">The book to verify.</param>
        /// <returns><c>True</c> if match, otherwise <c>False</c>.</returns>
        public bool Verify(Book.Book book);
    }
}
