using BookStoreManager.Admin;
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
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager manager;

        public AdminController(IAdminManager adminManager)
        {
            this.manager = adminManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Login login)
        {
            try
            {
                Task<string> response = this.manager.Login(login);
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Logged In Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Not Logged In", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

    }
}
