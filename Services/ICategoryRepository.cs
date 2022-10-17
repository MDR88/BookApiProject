using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    public interface ICategoryRepository
    {

        ICollection<Category> GetCategories(); // Returns all categories 
        Category GetCategory(int categoryId);  // returns single category
        ICollection<Category> GetAllCategoriesForABook(int bookId); // Gets specific category for book
        ICollection<Book> GetAllBooksForCategory(int categoryId); // retrieve all book categories from specific books

        bool CategoryExists(int catergoryId); // Input validation
        bool IsDuplicateCategoryName(int categoryId, string categoryName);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
