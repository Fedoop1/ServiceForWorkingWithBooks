using System.Collections.Generic;
using NUnit.Framework;

namespace ServiceForWorkingWithBooks.Tests
{
    internal static class BookSource
    {
        public static IEnumerable<TestCaseData> BooksForStorages
        {
            get
            {
                yield return new TestCaseData(new List<Book.Book>()
                {
                    Book.Book.Parse("\"Программирование на С# 5.0\",Иен Гриффитс.,1.17.2019,Какое-то издательство,884,9992158107, 49$"),
                    Book.Book.Parse("\"Effective CSharp Covers CSharp 6.0\",Wagner B.,8-23-2017,Какое-то издательство,600,0943396042, 49.5"),
                    Book.Book.Parse("\"CSharp 7.0 in a Nutshell\",Albahari J.,1/3/2019,O'reilly,1089,097522980X, 74.99"),
                    Book.Book.Parse("\"C# in Depth.\",Jon Skeet.,08.14.2014,Какое-то издательство,884,8090273416, 98.99$"),
                    Book.Book.Parse("\"Язык программирования C# 4.0\",Хейлсберг А.,1.1.2012,Какое-то издательство,784,9971502100, 50$")
                });
            }
        }
    }
}
