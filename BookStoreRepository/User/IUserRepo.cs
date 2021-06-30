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
        Task<UserDetails> AddUser(UserDetails user);
        Task<IEnumerable<string>> GetAllAddress(int userID);
        Task<int> AddNewAddress(int userID, string address);
        Task<UserDetails> GetUser(int userID);
    }
}
