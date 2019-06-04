using System.Collections.Generic;
using BookApi.Dtos;
using BookApi.Models;
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
        private readonly IReviewerRepository _reviewerRepository;
        public ReviewsController(IReviewRepository ReviewRepository, IReviewerRepository reviewerRepository, IBookRepository BookRepository)
        {
            _reviewerRepository = reviewerRepository;
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





        [HttpGet("{ReviewId}", Name = "GetReview")]
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
            if (!_ReviewRepository.ReviewExists(ReviewId))
                return NotFound();

            var Book = _ReviewRepository.GetBookOfAReview(ReviewId);

            if (!ModelState.IsValid)
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody]Review reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            if(!_reviewerRepository.ReviewerExists(reviewToCreate.Reviewer.Id))
                ModelState.AddModelError("","Reviewr doesn't exist.");

            if(!_BookRepository.BookExists(reviewToCreate.Book.Id))
                ModelState.AddModelError("","Book doesn't exist.");
            
            if(!ModelState.IsValid)
                return StatusCode(404, ModelState);
            
            reviewToCreate.Book = _BookRepository.GetBook(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);
        
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_ReviewRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("",$"Something went wrong saving the review");
                return StatusCode(500,ModelState);
            }
            return CreatedAtRoute("GetReview", new {ReviewId = reviewToCreate.Id}, reviewToCreate);
        }



        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview(int reviewId, [FromBody]Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if(reviewId !=reviewToUpdate.Id )
                return BadRequest(ModelState);
            
            if(!_ReviewRepository.ReviewExists(reviewId))
                ModelState.AddModelError("","Review doesn't exist.");

            if(!_reviewerRepository.ReviewerExists(reviewToUpdate.Reviewer.Id))
                ModelState.AddModelError("","Reviewer doesn't exist.");

            if(!_BookRepository.BookExists(reviewToUpdate.Book.Id))
                ModelState.AddModelError("","Book doesn't exist.");
            
            if(!ModelState.IsValid)
                return StatusCode(404, ModelState);
            
            reviewToUpdate.Book = _BookRepository.GetBook(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);
        
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_ReviewRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("",$"Something went wrong saving the review");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }



        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReview(int reviewId)
        {
            if(!_ReviewRepository.ReviewExists(reviewId))
                ModelState.AddModelError("","Review doesn't exist.");

            var reviewToDelete = _ReviewRepository.GetReview(reviewId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_ReviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("",$"Something went wrong saving the review");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }



    }
}