using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviews(int reviewId);
        Reviewer GetReviewsByReviewer(int reviewerId);
        Reviewer GetReviewerOfAReview(int reviewerId);
        bool ReviewerExists(int reviewerId);

    }
}
