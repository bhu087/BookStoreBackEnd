using BookStoreManager.Books;
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
    }
}
