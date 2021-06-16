using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRepository.User
{
    public interface IUserRepo
    {
        Task<string> Login(Login login);
    }
}
