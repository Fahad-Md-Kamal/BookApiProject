using System.Collections.Generic;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _ReviewRepository;
        private readonly IBookRepository _BookRepository;
        public ReviewsController(IReviewRepository ReviewRepository, IBookRepository BookRepository)
        {
            _BookRepository = BookRepository;
            _ReviewRepository = ReviewRepository;
        }

         

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _ReviewRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }
            return Ok(reviewsDto);
        }





        [HttpGet("{ReviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int ReviewId)
        {
            if (!_ReviewRepository.ReviewExists(ReviewId))
                return NotFound();

            var review = _ReviewRepository.GetReview(ReviewId);

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewDto = new ReviewDto()
            {
                Id = review.Id,
                Headline = review.Headline,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };
            return Ok(reviewDto);
        }






        [HttpGet("books/{BookId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsOfABook(int BookId)
        {
            if (!_BookRepository.BookExists(BookId))
                return NotFound();

            var reviews = _ReviewRepository.GetReviewsOfABook(BookId);

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }
            return Ok(reviewsDto);
        }






        [HttpGet("{ReviewId}/Book")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookOfAReview(int ReviewId)
        {
            if(!_ReviewRepository.ReviewExists(ReviewId))
                return NotFound();

            var Book = _ReviewRepository.GetBookOfAReview(ReviewId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
                Id = Book.Id,
                Isbn = Book.Isbn,
                Title = Book.Title,
                DatePublished = Book.DatePublished
            };

            return Ok(bookDto);
        }
    }
}