using BookApiProject.Dtos;
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
        public ReviewersController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        //api/reviewers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
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

        //api/reviewer/reviewers/reviewerId
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(400)]
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
    }
}
