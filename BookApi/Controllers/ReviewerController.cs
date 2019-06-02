using System.Collections.Generic;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{


    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private IReviewerRepository _ReviewerRepository { get; }
        private IReviewRepository _ReviewRepository { get; }

        public ReviewerController(IReviewerRepository ReviewerRepository, IReviewRepository ReviewRepository)
        {
            _ReviewRepository = ReviewRepository;
            _ReviewerRepository = ReviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewers()
        {
            var reviewrs = _ReviewerRepository.GetReviewers();

            var reviewrsDto = new List<ReviewerDto>();

            foreach (var reviewer in reviewrs)
            {
                reviewrsDto.Add(new ReviewerDto
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                });
            }
            return Ok(reviewrsDto);
        }


        [HttpGet("{ReviewerId}")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewer(int ReviewerId)
        {
            if (!_ReviewerRepository.ReviewerExists(ReviewerId))
                return NotFound();

            var reviewer = _ReviewerRepository.GetReviewer(ReviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);
        }




        [HttpGet("{ReviewerId}/Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsByReviewer(int ReviewerId)
        {
            if (!_ReviewerRepository.ReviewerExists(ReviewerId))
                return NotFound();

            var reviews = _ReviewerRepository.GetReviewsByReviewer(ReviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewesDto = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                reviewesDto.Add(new ReviewDto()
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return Ok(reviewesDto);
        }




        [HttpGet("{ReviewId}/Reviewer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewerByReview(int ReviewId)
        {
            if(!_ReviewRepository.ReviewExists(ReviewId))
                return NotFound();

            var reviewer = _ReviewerRepository.GetReviewerByReview(ReviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };

            return Ok(reviewerDto);

        }








    }
}