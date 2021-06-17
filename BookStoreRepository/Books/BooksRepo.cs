using BookStoreModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRepository.Books
{
    public class BooksRepo : IBooksRepo
    {
        private readonly IConfiguration config;
        public BooksRepo(IConfiguration configuration)
        {
            this.config = configuration;
        }
        public async Task<Book> AddNewBook(Book book)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddNewBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("Name", book.BookName);
                    command.Parameters.AddWithValue("Author", book.Author);
                    command.Parameters.AddWithValue("Price", book.Price);
                    command.Parameters.AddWithValue("Description", book.Description);
                    command.Parameters.AddWithValue("Quantity  ", book.Quantity);
                    command.Parameters.AddWithValue("Image", book.Image);
                    connection.Open();
                    int reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        connection.Close();
                        return await Task.Run(() => book);
                    }
                    connection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<Book> UpdateBook(Book book)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spUpdateBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("BookID", book.BookID);
                    command.Parameters.AddWithValue("BookName", book.BookName);
                    command.Parameters.AddWithValue("Description", book.Description);
                    command.Parameters.AddWithValue("Quantity  ", book.Quantity);
                    connection.Open();
                    int reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        connection.Close();
                        return await Task.Run(() => book);
                    }
                    connection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<string> DeleteBook(int bookID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spDeleteBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("BookID", bookID);
                    connection.Open();
                    int reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        connection.Close();
                        return await Task.Run(() => "Deleted");
                    }
                    connection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }


        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            string conn = config["ConnectionString"];
            List<Book> bookList = new List<Book>();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetAllBooks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Book book = new Book();
                        book.BookID = (Int32)reader["BookID"];
                        book.BookName = reader["BookName"].ToString();
                        book.Description = reader["BookDescription"].ToString();
                        book.Quantity = (Int32)reader["Quantity"];
                        bookList.Add(book);
                    }
                    connection.Close();
                    return await Task.Run(() => bookList);
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<int> AddToCart(int AccountID, int BookID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddToCart", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", AccountID);
                    command.Parameters.AddWithValue("BookID", BookID);
                    connection.Open();
                    int reader = await Task.Run(() => command.ExecuteNonQuery());
                    if (reader == 1)
                    {
                        connection.Close();
                        return reader;
                    }
                    connection.Close();
                    return reader;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<int> AddToWishList(int AccountID, int BookID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddToWishList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", AccountID);
                    command.Parameters.AddWithValue("BookID", BookID);
                    connection.Open();
                    int reader = await Task.Run(() => command.ExecuteNonQuery());
                    if (reader == 1)
                    {
                        connection.Close();
                        return reader;
                    }
                    connection.Close();
                    return reader;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<IEnumerable<Book>> PlaceOrder(int AccountID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetCart", connection))
                {
                    List<Book> orderList = new List<Book>();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", AccountID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Book bookDetails = new Book();
                            bookDetails.BookName = reader["BookName"].ToString();
                            bookDetails.Author = reader["Author"].ToString();
                            bookDetails.Price = (int)reader["Price"];
                            bookDetails.Quantity = (int)reader["Count"];
                            bookDetails.Price = bookDetails.Price * bookDetails.Quantity;
                            bookDetails.Description = reader["Address"].ToString();
                            orderList.Add(bookDetails);
                        }
                        return await Task.Run(() => orderList);
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        //public async Task<int> SortBooks(string sortOrder)
        //{
        //    //string conn = config["ConnectionString"];
        //    //SqlConnection connection = new SqlConnection(conn);
        //    try
        //    {
        //        IEnumerable<Book> booksList = this.GetAllBooks().Result;
        //        booksList.GetEnumerator.sortOrder
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception();
        //    }
        //}

    }
}
