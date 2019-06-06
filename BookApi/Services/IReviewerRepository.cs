using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IReviewerRepository
    {
        bool ReviewerExists(int ReviewerId);

        #region Get Operations
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int ReviewerId);
        ICollection<Review> GetReviewsByReviewer(int ReviewerId);
        Reviewer GetReviewerByReview(int ReviewId);
        #endregion

        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();
    }
}