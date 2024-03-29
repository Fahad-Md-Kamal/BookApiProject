using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IReviewRepository
    {
         bool ReviewExists(int ReviewId);
         ICollection<Review> GetReviews();
         Review GetReview(int ReviewId);
         ICollection<Review> GetReviewsOfABook(int BookId);
         Book GetBookOfAReview(int ReviewId);


         bool CreateReview(Review review);
         bool UpdateReview(Review review);
         bool DeleteReview(Review review);
         bool DeleteReviews(List<Review> reviews);
         bool Save();
    }
}