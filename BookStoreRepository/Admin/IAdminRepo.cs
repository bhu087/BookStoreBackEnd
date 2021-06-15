using BookStoreModel;
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
    }
}
