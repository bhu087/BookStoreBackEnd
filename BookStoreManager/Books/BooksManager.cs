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

        public Task<int> DecreaseFromCart(int AccountID, int BookID)
        {
            try
            {
                return this.repo.DecreaseFromCart(AccountID, BookID);
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

        public Task<IEnumerable<CartDetails>> GetCart(int AccountID)
        {
            try
            {
                return this.repo.GetCart(AccountID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<Book>> GetCartHistory(int userID)
        {
            try
            {
                return this.repo.GetCartHistory(userID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<string> PlaceOrder(int AccountID)
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

        public Task<IEnumerable<Book>> SortBooks(string sortOrder)
        {
            try
            {
                return this.repo.SortBooks(sortOrder);
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

        public Task<int> WishToCart(int AccountID, int BookID)
        {
            try
            {
                return this.repo.WishToCart(AccountID,BookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<int> DeleteFromCart(int AccountID, int BookID)
        {
            try
            {
                return this.repo.DeleteFromCart(AccountID, BookID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<CartDetails>> GetWishList(int AccountID)
        {
            try
            {
                return this.repo.GetWishList(AccountID);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<int> DeleteFromWishlist(int AccountID, int BookID)
        {
            try
            {
                return this.repo.DeleteFromWishlist(AccountID, BookID);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
