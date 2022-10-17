﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public class CountryRepository : ICountryRepository
    {
        // GET Methods
        private BookDbContext _countryContext;  // Brings in BookDbContext which impliments DbContext, Underscore indicates Private Variable

        public CountryRepository(BookDbContext countryContext) // Constructor brings instance of BookDbContext into class
        {

            _countryContext = countryContext; //Assigning to my variable
        }

        // Input validation check to verify matching Id
        public bool CountryExists(int countryId)
        {
            return _countryContext.Countries.Any(c => c.Id == countryId);
        }

        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {
            return _countryContext.Authors.Where(c => c.Country.Id == countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _countryContext.Countries.OrderBy(c => c.Name).ToList();  // Access my DbSet Countries, Lambda Expression to returns collection in alphabetical order
        }

        public Country GetCountry(int countryId)
        {
            return _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault(); // Lambda selects country that matches Country ID. FirstOrDefault method to account Null Values.
        }

        public Country GetCountryOfAnAuthor(int authorId)
        {
            return _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country = _countryContext.Countries.Where(c => c.Name.Trim().ToUpper() == countryName && c.Id != countryId);  

            return country == null ? false : true;
        }
    }
}
