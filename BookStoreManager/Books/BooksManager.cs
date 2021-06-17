using BookStoreModel;
using BookStoreRepository.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Books
{
    public class BooksManager : IBooksManager
    {
        private readonly IBooksRepo repo;

        public BooksManager(IBooksRepo booksRepo)
        {
            this.repo = booksRepo;
        }
        public Task<Book> AddNewBook(Book book)
        {
            return this.repo.AddNewBook(book);
        }

        public Task<int> AddToCart(int AccountID, int BookID)
        {
            try
            {
                return this.repo.AddToCart(AccountID, BookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<int> AddToWishList(int AccountID, int BookID)
        {
            try
            {
                return this.repo.AddToWishList(AccountID, BookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<string> DeleteBook(int bookID)
        {
            try
            {
                return this.repo.DeleteBook(bookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            try
            {
                return this.repo.GetAllBooks();
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<CartDetails>> PlaceOrder(int AccountID)
        {
            try
            {
                return this.repo.PlaceOrder(AccountID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<Book> UpdateBook(Book book)
        {
            try
            {
                return this.repo.UpdateBook(book);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
