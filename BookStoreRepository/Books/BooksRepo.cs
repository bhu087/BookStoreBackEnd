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

        public async Task<IEnumerable<CartDetails>> PlaceOrder(int AccountID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetCart", connection))
                {
                    List<CartDetails> orderList = new List<CartDetails>();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", AccountID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            CartDetails cartDetails = new CartDetails
                            {
                                BookID = (int)reader["BookID"],
                                CartID = (int)reader["CartID"],
                                BookName = reader["BookName"].ToString(),
                                Author = reader["Author"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (int)reader["Price"],
                                Quantity = (int)reader["Count"],
                                Image = reader["Image"].ToString()
                            };
                            if ((int)reader["Quantity"] == 0)
                            {
                                cartDetails.Quantity = 0;
                            }
                            cartDetails.Price *= cartDetails.Quantity;
                            orderList.Add(cartDetails);
                        }
                        if (this.UpdateQuantity(orderList) == 1)
                        {
                            return await Task.Run(() => orderList);
                        }
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

        public int UpdateQuantity(IEnumerable<CartDetails> cartDetails)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                foreach(var a in cartDetails)
                {
                    using (SqlCommand command = new SqlCommand("spUpdateBooksQuantity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("BookID", a.BookID);
                        command.Parameters.AddWithValue("Count", a.Quantity);
                        command.Parameters.AddWithValue("CartID", a.CartID);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
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
