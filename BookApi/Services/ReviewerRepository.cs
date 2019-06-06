using System.Collections.Generic;
using System.Linq;
using BookApi.Models;

namespace BookApi.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        public BookDbContext _Context;

        public ReviewerRepository(BookDbContext context)
        {
            _Context = context;
        }

        public Reviewer GetReviewer(int ReviewerId)
        {
            return _Context.Reviewers.Where(r => r.Id == ReviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerByReview(int ReviewId)
        {
            // var reviewerId = _Context.Reviews.Where(rv => rv.Id == ReviewId).Select(rvr => rvr.Reviewer.Id).FirstOrDefault();
            // return _Context.Reviewers.Where(rr => rr.Id == reviewerId).FirstOrDefault();
           
            return _Context.Reviews.Where(rv => rv.Id == ReviewId).Select(rvr => rvr.Reviewer).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _Context.Reviewers.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int ReviewerId)
        {
            return _Context.Reviews.Where(rvr => rvr.Reviewer.Id == ReviewerId).ToList();
        }

        public bool ReviewerExists(int ReviewerId)
        {
            return _Context.Reviewers.Any(rvr => rvr.Id == ReviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _Context.Add(reviewer);
            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _Context.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _Context.Remove(reviewer);
            return Save();
        }

        public bool Save()
        {
            var SaveItem = _Context.SaveChanges();
            return SaveItem >= 0 ? true : false;
        }
    }
}