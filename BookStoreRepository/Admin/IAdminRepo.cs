﻿using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRepository.Admin
{
    public interface IAdminRepo
    {
        Task<string> Login(Login login);
        Task<Book> AddNewBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<string> DeleteBook(int bookID);
        Task<IEnumerable<Book>> GetAllBooks();
    }
}
