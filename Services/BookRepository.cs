using BookApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Services
{
    public class BookRepository : IBookRepository
    {

        private BookDbContext _bookDbContext;
        public BookRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public bool BookExists(int bookId)
        {
            return _bookDbContext.Books.Any(b => b.Id == bookId);

        }

        public bool BookExists(string bookIsbn)
        {
            return _bookDbContext.Books.Any(b => b.Isbn == bookIsbn);
        }

        public Book GetBook(int bookId)
        {
            return _bookDbContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string bookIsbn)
        {
            return _bookDbContext.Books.Where(b => b.Isbn == bookIsbn).FirstOrDefault();
        }

        // NOTE: Each Book has a review, each review for a book has a rating.
        public decimal GetBookRating(int bookId)
        {
            var reviews = _bookDbContext.Reviews.Where(r => r.Book.Id == bookId); // Grab all reviews for book
            if (reviews.Count() <= 0)  // make sure at least 1 review doing review check
                return 0;
            // converted result explicitly to decimal to preserve int
            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count()); //If there are reviews, average all ratings for it. Divide by the count in reviews.
        }

        public ICollection<Book> GetBooks(int bookId)
        {
            return _bookDbContext.Books.OrderBy(b => b.Title).ToList();
        }

        // Check bookID and Isbn updating to is matching another bookId with same Isbn
        public bool IsDuplicateIsbn(int bookId, string bookIsbn)
        {
            var book = _bookDbContext.Books.Where(b => b.Isbn.Trim().ToUpper() == bookIsbn && b.Id != bookId);  //Check if book Isbn has same bookId

            return book == null ? false : true;
        }
    }
}
