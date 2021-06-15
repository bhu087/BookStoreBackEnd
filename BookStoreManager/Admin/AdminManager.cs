﻿using BookStoreModel;
using BookStoreRepository.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Admin
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepo adminRepo;

        public AdminManager(IAdminRepo repo)
        {
            this.adminRepo = repo;
        }

        public Task<Book> AddNewBook(Book book)
        {
            return this.adminRepo.AddNewBook(book);
        }

        public Task<string> DeleteBook(int bookID)
        {
            try
            {
                return this.adminRepo.DeleteBook(bookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            try
            {
                return this.adminRepo.GetAllBooks();
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<string> Login(Login login)
        {
            return this.adminRepo.Login(login);
        }

        public Task<Book> UpdateBook(Book book)
        {
            try
            {
                return this.adminRepo.UpdateBook(book);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
