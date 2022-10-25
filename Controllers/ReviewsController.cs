using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BookApiProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;
        public ReviewsController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IBookRepository bookRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
        }

        //api/reviews
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewesDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewesDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return Ok(reviewesDto);
        }

        //api/reviews/reviewId
        [HttpGet("{reviewId}",Name = "GetReview")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {

            if (!_reviewRepository.ReviewExists(reviewId))

                return NotFound();

            var review = _reviewRepository.GetReview(reviewId); 

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                Headline = review.Headline,
                Rating = review.Rating,
                ReviewText = review.ReviewText
               
            };
            return Ok(reviewDto);
        }


        //api/reviews/books/bookId
        [HttpGet("books/{bookId}")]  // Passing in reviewerId to validate if it exists /reviews to make the URI unique
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsForABook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
                return NotFound();

            var reviews = _reviewRepository.GetReviewsOfABook(bookId);
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return Ok(reviewsDto);
        }

        //api/reviews/reviewId/book
        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var book = _reviewRepository.GetBookOfAReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
             Id = book.Id,
             Title = book.Title,
             Isbn = book.Isbn,
             DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }

        //api/reviews
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))] // Created Ok 
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(404)]  // Book not found
        [ProducesResponseType(500)]  // Server Error
        public IActionResult CreateReview([FromBody] Review reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewToCreate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer doesn't exist!");

            if (!_bookRepository.BookExists(reviewToCreate.Book.Id))
                ModelState.AddModelError("", "Book doesn't exist!");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewToCreate.Book = _bookRepository.GetBook(reviewToCreate.Reviewer.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);

            if (!_reviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review.");
                return StatusCode(500, ModelState); 
            }

            return CreatedAtRoute("GetReview", new { reviewId = reviewToCreate.Id }, reviewToCreate);  //Created new categoryId matching newly created Id of the object
        }

        //api/reviews/reviewId
        [HttpPut("{reviewId}")]
        [ProducesResponseType(204, Type = typeof(Review))] // No Content
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(404)]  // Book not found
        [ProducesResponseType(500)]  // Server Error
        public IActionResult UpdateReview(int reviewId, [FromBody] Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);

            if(!_reviewRepository.ReviewExists(reviewId))
                ModelState.AddModelError("", "Review doesn't exist!");

            if (!_reviewerRepository.ReviewerExists(reviewToUpdate.Reviewer.Id))
                ModelState.AddModelError("", "Reviewer doesn't exist!");

            if (!_reviewerRepository.ReviewerExists(reviewToUpdate.Book.Id))
                ModelState.AddModelError("", "Book doesn't exist!");           

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewToUpdate.Book = _bookRepository.GetBook(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review.");
                return StatusCode(500, ModelState);
            }

            return NoContent();  //Created new categoryId matching newly created Id of the object
        }


        //api/reviews/reviewId
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]  //No Content
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(404)]  // Not Found
        [ProducesResponseType(409)]  // Conflict
        [ProducesResponseType(500)]  // Server Error
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting review.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
