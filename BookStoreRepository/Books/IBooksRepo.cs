using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRepository.Books
{
    public interface IBooksRepo
    {
        Task<Book> AddNewBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<string> DeleteBook(int bookID);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<int> AddToCart(int AccountID, int BookID);
        Task<int> AddToWishList(int AccountID, int BookID);
        Task<IEnumerable<CartDetails>> PlaceOrder(int AccountID);
    }
}
