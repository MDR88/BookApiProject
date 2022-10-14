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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        //api/categories
        [HttpGet]
        [ProducesResponseType(400)] // Helps when debugging
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))] // Helps when debugging
        public IActionResult GetCategories() // Actions return IActionResult
        {
            var categories = _categoryRepository.GetCategories().ToList();
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          
            var categoriesDto = new List<CategoryDto>();
          
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDto);
        }

        //api/categories/categoryId
        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategory(categoryId);  

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDto = new CategoryDto()
            {

                Id = category.Id,
                Name = category.Name

            };

            return Ok(categoryDto);
        }

        //api/categories/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))] // IEnumerable because returning list
        public IActionResult GetAllCategoriesForABook(int bookId)
        {
            // TO DO - Validate the Book exists

            var categories = _categoryRepository.GetAllCategoriesForABook(bookId);  // Returns List Of Categories that belong bookId and assigns to categories

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoriesDto = new List<CategoryDto>();

            foreach(var category in categories)   // To construct the Dto object in the list 
            {
                categoriesDto.Add(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name

                });

            }       

            return Ok(categoriesDto);
        }

        //api/categories/categoryId/books
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(404)]  // For Not Found response
        [ProducesResponseType(400)]  // For Not Found response
        public IActionResult GetAllBooksForCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var books = _categoryRepository.GetAllBooksForCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                  Id = book.Id,
                  Title = book.Title,
                  Isbn = book.Isbn,
                  DatePublished = book.DatePublished

                });
            }

            return Ok(booksDto);

        }

    }

}