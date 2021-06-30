using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Books
{
    public interface IBooksManager
    {
        Task<Book> AddNewBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<string> DeleteBook(int bookID);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<int> AddToCart(int AccountID, int BookID);
        Task<int> AddToWishList(int AccountID, int BookID);
        Task<string> PlaceOrder(int AccountID);
        Task<int> WishToCart(int AccountID, int BookID);
        Task<IEnumerable<Book>> SortBooks(string sortOrder);
        Task<IEnumerable<CartDetails>> GetCart(int AccountID);
        Task<IEnumerable<Book>> GetCartHistory(int userID);
        Task<int> DecreaseFromCart(int AccountID, int BookID);
        Task<int> DeleteFromCart(int AccountID, int BookID);
        Task<IEnumerable<CartDetails>> GetWishList(int AccountID);
        Task<int> DeleteFromWishlist(int AccountID, int BookID);
    }
}
