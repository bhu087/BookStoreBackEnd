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
