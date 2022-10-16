using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Dtos;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;


//API Call
namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;  //Import/Inject the interface into the class, Because it's an interface need to instantiate concrete object.
        private IAuthorRepository _authorRepository;
        public CountriesController(ICountryRepository countryRepository, IAuthorRepository authorRepository)
        {
            _countryRepository = countryRepository;
            _authorRepository = authorRepository;

        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)] // Helps when debugging
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))] // Helps when debugging
        public IActionResult GetCountries() // Actions return IActionResult
        {
            var countries = _countryRepository.GetCountries().ToList();
            // added validation check if valid, if invalid return 404 bad request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Created a new list to populate countries ID and Name properties only
            var countriesDto = new List<CountryDto>();
            // Loop through countries list to populate ID and name to new list
            foreach (var country in countries)
            {
                //each ideratrion will add new countryDto Object with ID and Name
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return Ok(countriesDto);
        }

        //api/countries/countryId
        [HttpGet("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto() {

                Id = country.Id,
                Name = country.Name

            };

            return Ok(countryDto);
        }

        // TODO - Need to test after implementing IAuthor repo
        //api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            var country = _countryRepository.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        //api/countries/countryId/authors
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(200, Type = typeof (IEnumerable<AuthorDto>))]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(400)]  // For Not Found response
        public IActionResult GetAuthorsFromACountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();
            var authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto 
                {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
                
                });
            }

            return Ok(authorsDto);

        }
    }

}
   
