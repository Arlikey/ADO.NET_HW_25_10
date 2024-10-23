using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_25_10
{
    public class AdditionalTask2
    {
        private static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=EFCoreDB;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            /*IEnumerable<object> books = GetBooksByAuthor(new Author() { Id = 1 });
            DeleteCheapiestBookInCategory(new Category() { Id = 1 });
            //IncreaseBooksPrice();
            var books = GetBooksByPriceRange(50, 100);
            var bookCount = GetBookCountPerAuthor();*/
        }

        static IEnumerable<object> GetBooksByAuthor(Author author)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = """
                    SELECT Title, a.FullName, c.Name, Price, PublishedOn
                    FROM Books
                    JOIN Authors as a ON a.Id = AuthorId
                    JOIN Categories as c ON c.Id = CategoryId
                    WHERE a.Id = @Id
                    """;
                return db.Query(sqlQuery, author);
            }
        }

        static void DeleteCheapiestBookInCategory(Category category)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = """
                    DELETE FROM Books
                    WHERE Id = (
                        SELECT TOP 1 Id
                        FROM Books
                        WHERE CategoryId = @Id
                        ORDER BY Price
                    )
                    """;
                db.Execute(sqlQuery, category);
            }
        }

        static void IncreaseBooksPrice()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = """
                    UPDATE Books SET Price = Price * 1.05
                    """;
                db.Execute(sqlQuery);
            }
        }

        static IEnumerable<Book> GetBooksByPriceRange(int start, int end)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = """
                    SELECT * FROM Books
                    WHERE Price BETWEEN @start AND @end
                    """;
                return db.Query<Book>(sqlQuery, new { start, end });
            }
        }

        static IEnumerable<dynamic> GetBookCountPerAuthor()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = """
                    SELECT a.Id AS AuthorId, a.FullName AS AuthorName, COUNT(b.Id) AS BooksCount
                    FROM Authors a
                    JOIN Books b ON a.Id = b.AuthorId
                    GROUP BY a.Id, a.FullName
                    """;
                return db.Query(sqlQuery);
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Book> Books { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
