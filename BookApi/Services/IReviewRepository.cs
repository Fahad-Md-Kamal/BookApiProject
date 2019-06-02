using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IReviewRepository
    {
         bool ReviewExists(int ReviewId);
         ICollection<Review> GetReviews();
         Review GetReview();
         ICollection<Review> GetReviewsOfABook(int BookId);
         Book GetBookOfAReview(int ReviewId);
    }
}