using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Dtos;
using BookApiProject.Models;
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
        [HttpGet("{countryId}", Name ="GetCountry")]  //NOTE: Had to specifiy explicitly enter for Create 
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)]  //Not Found
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
        
        //api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)]  // Not Found
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
        [ProducesResponseType(404)]  // Not Found
        [ProducesResponseType(400)]  // Bad Request
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


        //api/countries
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))] // Created Ok 
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(422)]  // Unprocessable Entity
        [ProducesResponseType(500)]  // Server Error
        public IActionResult CreateCountry([FromBody]Country countryToCreate)
        {
            if (countryToCreate == null)            
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault(); //Check for duplicate

            if (country != null)
            {
                ModelState.AddModelError("", $"Country {countryToCreate.Name} already exists");
                return StatusCode(422, ModelState);  //Unprocessable Entity, also returning ModelState for debugging
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            if (!_countryRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countryToCreate.Name} already exists");
                return StatusCode(500, ModelState); // Server error
            }

            return CreatedAtRoute("GetCountry", new { countryId = countryToCreate.Id }, countryToCreate);  //Created new countryId matching newly created Id of the object
        }

        //api/countries/countryId
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]  //No Content
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(404)]  // Not Found
        [ProducesResponseType(422)]  // Unprocessable Entity
        [ProducesResponseType(500)]  // Server Error
        public IActionResult UpdateCountry(int countryId, [FromBody]Country updatedCountryInfo)
        {
            if (updatedCountryInfo == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountryInfo.Id)  // Checking if countryId matches updatedCountryInfo
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (_countryRepository.IsDuplicateCountryName(countryId, updatedCountryInfo.Name))
            {
                ModelState.AddModelError("", $"Country {updatedCountryInfo.Name} already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.UpdateCountry(updatedCountryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedCountryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/countryId
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]  //No Content
        [ProducesResponseType(400)]  // Bad Request
        [ProducesResponseType(404)]  // Not Found
        [ProducesResponseType(422)]  // Unprocessable Entity
        [ProducesResponseType(409)]  // Conflict
        [ProducesResponseType(500)]  // Server Error
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var countryToDelete = _countryRepository.GetCountry(countryId);  // calling GetCountry method if country exists

            if (_countryRepository.GetAuthorsFromACountry(countryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Country {countryToDelete.Name} " +
                                        "cannot be deleted because it is used by at least one author");
                return StatusCode(409, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}
   
