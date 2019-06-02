using System.Collections.Generic;
using System.Linq;
using BookApi.Models;

namespace BookApi.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookDbContext _Context;
        public ReviewRepository(BookDbContext Context)
        {
            _Context = Context;
        }

        public Book GetBookOfAReview(int ReviewId)
        {
           return _Context.Reviews.Where(rv => rv.Id == ReviewId).Select(b => b.Book).FirstOrDefault();
        }

        public Review GetReview(int ReviewId)
        {
            return _Context.Reviews.Where(rv => rv.Id == ReviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _Context.Reviews.OrderBy(rv => rv.Rating).ToList();
        }

        public ICollection<Review> GetReviewsOfABook(int BookId)
        {
            return _Context.Reviews.Where(bk => bk.Book.Id == BookId).ToList(); 
            // we need to go inside the book class from the review table 
            // to grab all the reviews related to the book
        }

        public bool ReviewExists(int ReviewId)
        {
            return _Context.Reviews.Any(rv => rv.Id == ReviewId);
        }
    }
}