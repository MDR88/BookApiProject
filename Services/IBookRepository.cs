﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApiProject.Models;

namespace BookApiProject.Services
{
    interface IBookRepository
    {
        ICollection<Book> GetBooks(int bookId);
        Book GetBook(int bookId);
        Book GetBook(string bookIsbn);
        decimal GetBookRating(int bookId);
        bool BookExists(int bookId);
        bool BookExistis(string bookIsbn);
        bool IsDuplicateIsbn(int bookId, string bookIsbn);
    }
}