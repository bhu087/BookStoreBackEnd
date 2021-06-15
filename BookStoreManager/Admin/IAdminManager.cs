using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Admin
{
    public interface IAdminManager
    {
        Task<string> Login(Login login);
    }
}
