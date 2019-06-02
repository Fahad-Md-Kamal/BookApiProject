using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IReviewerRepository
    {
         bool ReviewerExists(int ReviewerId);
         ICollection<Reviewer> GetReviewers();
         Reviewer GetReviewer(int ReviewerId);
         ICollection<Review> GetReviewsByReviewer(int ReviewerId);
         Reviewer GetReviewerByReview(int ReviewId);
    }
}