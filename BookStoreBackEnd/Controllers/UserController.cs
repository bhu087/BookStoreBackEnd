using BookStoreManager.User;
using BookStoreModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserManager manager;
        public UserController(IUserManager userManager)
        {
            this.manager = userManager;
        }

        [HttpPost]
        [Route("userLogin")]
        public ActionResult Login(Login login)
        {
            try
            {
                Task<string> response = this.manager.Login(login);
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "User Logged In Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "User Not Logged In", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [HttpPost]
        public ActionResult Register()
        {
            try
            {
                Task<string> response = null;
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "User Registered Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "User Not Registered", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }
    }
}
