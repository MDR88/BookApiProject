﻿using BookApiProject.Dtos;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewersController : Controller 
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;  // Needed to access if reviewerId with a review exists
        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/reviewers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewersDto = new List<ReviewerDto>();

            foreach (var reviewer in reviewers)
            {
                reviewersDto.Add(new ReviewerDto
                { 
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    Lastname = reviewer.LastName
                });
            }
            return Ok(reviewersDto);
        }

        //api/reviewer/reviewerId
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
          
                return NotFound();

                var reviewer = _reviewerRepository.GetReviewer(reviewerId);  // Calling repo, if not valid return bad request

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                Lastname = reviewer.LastName,
            };
            return Ok(reviewerDto);        
        }

        //api/reviewers/reviewerId/
        [HttpGet("{reviewerId}/reviews")]  // Passing in reviewerId to validate if it exists /reviews to make the URI unique
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))

                return NotFound();

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto()
                { 
                Id = review.Id,
                Headline = review.Headline,
                Rating = review.Rating,
                ReviewText = review.ReviewText
                });
            }
            return Ok(reviewsDto);
        }

        // NOTE: Need to validate if review with this Id exists, Need to call ReviewExist in the Review repository
        // Must inject reviewRepository into controller above

        // TODO - Need to test after implement the IReview repository
        //api/reviewers/reviewId
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerDto = new ReviewerDto()
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                Lastname = reviewer.LastName,
            };

            return Ok(reviewerDto);
        }
    }
}
