using BookStoreModel;
using BookStoreRepository.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.User
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepo repo;
        public UserManager(IUserRepo userRepo)
        {
            this.repo = userRepo;
        }

        public Task<int> AddNewAddress(int userID, string address)
        {
            try
            {
                return this.repo.AddNewAddress(userID, address);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<UserDetails> AddUser(UserDetails user)
        {
            try 
            {
                return this.repo.AddUser(user);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<string>> GetAllAddress(int userID)
        {
            try
            {
                return this.repo.GetAllAddress(userID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<string> Login(Login login)
        {
            try
            {
                return this.repo.Login(login);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
