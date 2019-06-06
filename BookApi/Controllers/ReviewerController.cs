using System.Collections.Generic;
using System.Linq;
using BookApi.Dtos;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{


    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewersController : Controller
    {

    # region Properties

        private IReviewerRepository _ReviewerRepository { get; }
        private IReviewRepository _ReviewRepository { get; }

        #endregion

    #region Constructor
        public ReviewersController(IReviewerRepository ReviewerRepository, IReviewRepository ReviewRepository)
        {
            _ReviewRepository = ReviewRepository;
            _ReviewerRepository = ReviewerRepository;
        }

        #endregion

    #region GetActions

        #region GetReviewers

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

            #endregion

        #region GetReviewer

            [HttpGet("{ReviewerId}", Name = "GetReviewer")]
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

        #endregion

        #region GetReviewsByReviewer

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

            #endregion
        
        #region GetReviewerByReview

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

        #endregion
    
    #endregion

    #region CreateReviewer


    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateReviewer([FromBody]Reviewer reviewerToCreate)
    {
        if(reviewerToCreate == null)
            return BadRequest(ModelState);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if(!_ReviewerRepository.CreateReviewer(reviewerToCreate))
        {
            ModelState.AddModelError("", $"Something went wrong while registrating {reviewerToCreate.FirstName} {reviewerToCreate.LastName}");
            return StatusCode(500,ModelState);
        }
        return CreatedAtRoute("GetReviewer", new {reviewerid = reviewerToCreate.Id}, reviewerToCreate);
    }

    #endregion

    #region UpdateReviewer

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody]Reviewer reviewerToUpdate)
        {
            if(reviewerToUpdate == null)
                return BadRequest(ModelState);
            
            if(reviewerId != reviewerToUpdate.Id)
                return BadRequest(ModelState);

            if(!_ReviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            if(!_ReviewerRepository.UpdateReviewer(reviewerToUpdate))
            {
                ModelState.AddModelError("",$"Something went wrong updating {reviewerToUpdate.FirstName} {reviewerToUpdate.LastName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    #endregion



        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if(!_ReviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            
            var reviewerToDelete = _ReviewerRepository.GetReviewer(reviewerId);
            var reviewrsToDelete = _ReviewerRepository.GetReviewsByReviewer(reviewerId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);   
            
            if(!_ReviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("",$"Something went wrong deleting {reviewerToDelete.FirstName} {reviewerToDelete.LastName}");
                return StatusCode(500, ModelState);
            } 
            
            if(!_ReviewRepository.DeleteReviews(reviewrsToDelete.ToList()))
            {
                ModelState.AddModelError("",$"Something went wrong deleting reviews of {reviewerToDelete.FirstName} {reviewerToDelete.LastName}");
                return StatusCode(500, ModelState);
            }         
            return NoContent();
        }

    }
}