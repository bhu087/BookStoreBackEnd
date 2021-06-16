﻿using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Books
{
    public interface IBooksManager
    {
        Task<Book> AddNewBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<string> DeleteBook(int bookID);
        Task<IEnumerable<Book>> GetAllBooks();
    }
}
