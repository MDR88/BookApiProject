using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries(); //Returning all countries
        Country GetCountry(int countryId); // Return single country
        Country GetCountryOfAnAuthor(int authorId);  // Gets specific country for an Author Method
        ICollection<Author> GetAuthorsFromACountry(int countryId); // Retrieve all Authors from Specific Country
        bool CountryExists(int countryId); // Input validation To confirm that the country exists 
    }
}
