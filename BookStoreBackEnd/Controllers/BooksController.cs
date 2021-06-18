using BookStoreManager.Books;
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
    public class BooksController : Controller
    {
        private readonly IBooksManager manager;

        public BooksController(IBooksManager adminManager)
        {
            this.manager = adminManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("addNewBook")]
        public ActionResult AddNewBook(Book book)
        {
            try
            {
                Task<Book> response = this.manager.AddNewBook(book);
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = " Book Added Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Book Not Added", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPut]
        //[Route("updateBook")]
        //public ActionResult UpdateBook(Book book)
        //{
        //    try
        //    {
        //        Task<Book> response = this.manager.UpdateBook(book);
        //        if (response.Result != null)
        //        {
        //            return this.Ok(new { Status = true, Message = " Book updated Successfully", Data = response.Result });
        //        }

        //        return this.BadRequest(new { Status = false, Message = "Book updated", Data = response.Result });
        //    }
        //    catch (Exception e)
        //    {
        //        return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
        //    }
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpDelete]
        //[Route("deleteBook")]
        //public ActionResult DeleteteBook(int bookID)
        //{
        //    try
        //    {
        //        Task<string> response = this.manager.DeleteBook(bookID);
        //        if (response.Result != null)
        //        {
        //            return this.Ok(new { Status = true, Message = " Book deleted Successfully", Data = response.Result });
        //        }

        //        return this.BadRequest(new { Status = false, Message = "Book dletetion failed", Data = response.Result });
        //    }
        //    catch (Exception e)
        //    {
        //        return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
        //    }
        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("getAllBooks")]
        public ActionResult GetAllBooks()
        {
            try
            {
                Task<IEnumerable<Book>> response = this.manager.GetAllBooks();
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "All Book", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "No books available", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("addToCart")]
        public ActionResult AddToCart(int BookID)
        {
            int userID = this.GetUserID();   
            try
            {
                Task<int> response = this.manager.AddToCart(userID, BookID);
                if (response.Result == 1)
                {
                    return this.Ok(new { Status = true, Message = "Book added to Cart", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Book not added to cart", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("addToWishList")]
        public ActionResult AddToWishList(int BookID)
        {
            int userID = this.GetUserID();
            try
            {
                Task<int> response = this.manager.AddToWishList(userID, BookID);
                if (response.Result == 1)
                {
                    return this.Ok(new { Status = true, Message = "Book added to wish List", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Book not added to wish list", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("addWishToCart")]
        public ActionResult WishToCart(int BookID)
        {
            int userID = this.GetUserID();
            try
            {
                Task<int> response = this.manager.WishToCart(userID, BookID);
                if (response.Result == 1)
                {
                    return this.Ok(new { Status = true, Message = "Book added to Cart", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Book not added to Cart", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("placeOrder")]
        public ActionResult PlaceOrder()
        {
            int userID = this.GetUserID();
            try
            {
                Task<IEnumerable<CartDetails>> response = this.manager.PlaceOrder(userID);
                
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "order placed Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Order not placed", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [Authorize(Roles ="User")]
        [HttpGet]
        [Route("orderByPrice")]
        public ActionResult SortBooks(string sortingOrder)
        {
            try
            {
                Task<IEnumerable<Book>> response = this.manager.SortBooks(sortingOrder);

                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "order placed Successfully", Data = response.Result });
                }

                return this.BadRequest(new { Status = false, Message = "Order not placed", Data = response.Result });
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
