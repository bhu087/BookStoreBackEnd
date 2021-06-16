﻿using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.User
{
    public interface IUserManager
    {
        Task<string> Login(Login login);
    }
}