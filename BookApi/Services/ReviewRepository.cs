using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public class ReviewRepository : IReviewRepository
    {

        
        public Book GetBookOfAReview(int ReviewId)
        {
            throw new System.NotImplementedException();
        }

        public Review GetReview()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Review> GetReviews()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Review> GetReviewsOfABook(int BookId)
        {
            throw new System.NotImplementedException();
        }

        public bool ReviewExists(int ReviewId)
        {
            throw new System.NotImplementedException();
        }
    }
}