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
        public CountriesController(ICountryRepository countryRepository) 
        {
            _countryRepository = countryRepository;         

        }

        //api/countries
        [HttpGet]
        public IActionResult GetCountries() // Actions return IActionResult
        {
            var countries = _countryRepository.GetCountries().ToList();

            // Created a new list to populate countries ID and Name properties
            var countriesDto = new List<CountryDto>();
            // Loop through countries list to populate ID and name to new list
            foreach(var country in countries)
            {
                //each ideratrion will add each Id and Name to countriesDto List
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return Ok(countries);
        }


    }

}
   
