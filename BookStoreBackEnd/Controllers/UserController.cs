using BookStoreManager.User;
using BookStoreModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        [Route("register")]
        public ActionResult Register(UserDetails user)
        {
            try
            {
                Task<UserDetails> response = this.manager.AddUser(user);
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

        [HttpPost]
        [Route("addAddress")]
        public ActionResult AddNewAddress(string address)
        {
            int userID = this.GetUserID();
            try
            {
                Task<int> response = this.manager.AddNewAddress(userID, address);
                if (response.Result != 0)
                {
                    return this.Ok(new { Status = true, Message = "Address Added Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Address Not added", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [HttpGet]
        [Route("addressList")]
        public ActionResult AllAddress()
        {
            int userID = this.GetUserID();
            try
            {
                Task<IEnumerable<string>> response = this.manager.GetAllAddress(userID);
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Your Address List", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "You dont have any address add new address", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }
        private int GetUserID()
        {
            var token = HttpContext.Request?.Headers["Authorization"];
            string tokenString = token.ToString();
            string[] tokenArray = tokenString.Split(" ");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenArray[1]);
            var tokenS = jsonToken as JwtSecurityToken;
            int userID = int.Parse(tokenS.Claims.First(claim => claim.Type == "Id").Value);
            return userID;
        }
    }
}
