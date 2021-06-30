using BookStoreModel;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        private MessageQueue messageQueue = new MessageQueue();
        public string ConnectionString()
        {
            return config["ConnectionString"];
        }

        public async Task<Book> AddNewBook(Book book)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddNewBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("BookName", book.BookName);
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
            string conn = this.ConnectionString();
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
            string conn = this.ConnectionString();
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
            string conn = this.ConnectionString();
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
                        Book book = new Book
                        {
                            BookID = (int)reader["BookID"],
                            Author = reader["Author"].ToString(),
                            BookName = reader["BookName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            Price = (int)reader["Price"],
                            Image = reader["Image"].ToString()
                        };
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
            string conn = this.ConnectionString();
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

        public async Task<int> DecreaseFromCart(int AccountID, int BookID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("decreaseFromCart", connection))
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

        public async Task<int> DeleteFromCart(int AccountID, int BookID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spDeleteFromCart", connection))
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

        public async Task<int> DeleteFromWishlist(int AccountID, int BookID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spDeleteFromWishlist", connection))
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
            string conn = this.ConnectionString();
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

        public async Task<string> PlaceOrder(int AccountID)
        {
            string conn = this.ConnectionString();
            string orderID = string.Empty;
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
                                Email = reader["Email"].ToString(),
                                BookName = reader["BookName"].ToString(),
                                Author = reader["Author"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (int)reader["Price"],
                                Quantity = (int)reader["Count"],
                                TotalPrice = (int)reader["Price"] * (int)reader["Count"],
                                Image = reader["Image"].ToString()
                            };
                            if ((int)reader["Quantity"] == 0)
                            {
                                cartDetails.Quantity = 0;
                            }
                            orderList.Add(cartDetails);
                        }
                        if (this.UpdateQuantity(orderList) == 1)
                        {
                            orderID = "#Order" + AccountID + orderList.First().BookID + orderList.Last().BookID;
                            this.SaveToCartHistory(orderList, AccountID, orderID);
                            string subject = "Order Details";
                            string body = string.Empty;
                            string email = string.Empty;
                            int grandTotal = 0;
                            foreach (var orders in orderList)
                            {
                                email = orders.Email;
                                body += "Book Name: " + orders.BookName
                                    + "\nBook Author: "+ orders.Author
                                    + "\nBook Price: "+ orders.Price
                                    + "\nTotal Price: (Rs." + orders.Price +"*" + orders.Quantity + 
                                    ") = " + orders.TotalPrice +"\n---------------------------------\n";
                                grandTotal += orders.TotalPrice;
                            }
                            body += "Grand Amount = " + grandTotal;
                            this.MsmqService();
                            this.AddToQueue(email, subject, body);
                            return await Task.Run(() => orderID);
                        }
                    }
                    return null;
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }


        public async Task<IEnumerable<CartDetails>> GetCart(int AccountID)
        {
            string conn = this.ConnectionString();
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
                                Email = reader["Email"].ToString(),
                                BookName = reader["BookName"].ToString(),
                                Author = reader["Author"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (int)reader["Price"],
                                Quantity = (int)reader["Count"],
                                TotalPrice = (int)reader["Price"] * (int)reader["Count"],
                                Image = reader["Image"].ToString()
                            };
                            orderList.Add(cartDetails);
                        }
                        if (orderList.Count() > 0)
                        {
                            return await Task.Run(() => orderList);
                        }
                    }
                    return null;
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<IEnumerable<CartDetails>> GetWishList(int AccountID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetWishList", connection))
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
                                Email = reader["Email"].ToString(),
                                BookName = reader["BookName"].ToString(),
                                Author = reader["Author"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (int)reader["Price"],
                                Quantity = (int)reader["Count"],
                                TotalPrice = (int)reader["Price"] * (int)reader["Count"],
                                Image = reader["Image"].ToString()
                            };
                            orderList.Add(cartDetails);
                        }
                        if (orderList.Count() > 0)
                        {
                            return await Task.Run(() => orderList);
                        }
                    }
                    return null;
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<int> WishToCart(int AccountID, int BookID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spWishToCart", connection))
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
        public int UpdateQuantity(IEnumerable<CartDetails> cartDetails)
        {
            string conn = this.ConnectionString();
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


        public async Task<IEnumerable<Book>> SortBooks(string sortOrder)
        {
            IEnumerable<Book> sortedBooks = this.GetAllBooks().Result;
            try
            {
                IEnumerable<Book> booksList = this.GetAllBooks().Result;
                if (sortOrder.Equals("LowToHigh"))
                {
                    sortedBooks = booksList.OrderBy(c => c.Price);
                }
                if(sortOrder.Equals("HighToLow"))
                {
                    sortedBooks = booksList.OrderByDescending(c => c.Price);
                }
                return await Task.Run(() => sortedBooks);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void SaveToCartHistory(IEnumerable<CartDetails> cartDetails, int accountID, string orderID)
        {
            string conn = this.ConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                SqlCommand command = new SqlCommand("spAddToHistory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AccountID", SqlDbType.Int);
                command.Parameters.Add("@BookID", SqlDbType.Int);
                command.Parameters.Add("@Price", SqlDbType.Int);
                command.Parameters.Add("@Quantity", SqlDbType.Int);
                command.Parameters.Add("@OrderID", SqlDbType.VarChar);
                connection.Open();
                foreach (var book in cartDetails)
                 {
                    command.Parameters[0].Value = accountID;
                    command.Parameters[1].Value = book.BookID;
                    command.Parameters[2].Value = book.Price;
                    command.Parameters[3].Value = book.Quantity;
                    command.Parameters[4].Value = orderID;
                    command.ExecuteNonQuery();
                }
                connection.Close();
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

        public async Task<IEnumerable<Book>> GetCartHistory(int userID)
        {
            string conn = this.ConnectionString();
            List<Book> oederHistroy = new List<Book>();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetCartHistory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", userID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            BookID = (int)reader["BookID"],
                            BookName = reader["BookName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            Price = (int)reader["Price"]
                        };
                        oederHistroy.Add(book);
                    }
                    connection.Close();
                    return await Task.Run(() => oederHistroy);
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

        public MessageQueue MsmqService()
        {
            string queuePath = @".\private$\BookStoreQueue";
            if (MessageQueue.Exists(queuePath))
            {
                this.messageQueue = new MessageQueue(queuePath);
                return this.messageQueue;
            }
            else
            {
                this.messageQueue = MessageQueue.Create(queuePath);
                return this.messageQueue;
            }
        }
        public void SendMail(string subject, string body)
        {
            try
            {
                string accountEmail = this.config["NetworkCredentials:AccountEmail"];
                string accountPass = this.config["NetworkCredentials:AccountPass"];
                MailMessage mail = new MailMessage();
                mail.To.Add("bhu087@gmail.com");
                mail.From = new MailAddress("bhush097@gmail.com");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(accountEmail, accountPass)
                };
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void AddToQueue(string email, string subject, string body)
        {
            EmailDetails emailDetails = new EmailDetails
            {
                Email = email,
                Subject = subject,
                Body = body
            };
            this.messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(EmailDetails) });

            this.messageQueue.ReceiveCompleted += this.ReceiveFromQueue;

            this.messageQueue.Send(emailDetails);

            this.messageQueue.BeginReceive();

            this.messageQueue.Close();
        }

        public void ReceiveFromQueue(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = this.messageQueue.EndReceive(e.AsyncResult);
                var emailDetails = (EmailDetails)msg.Body;
                this.SendMail(emailDetails.Subject, emailDetails.Body);
                using (StreamWriter file = new StreamWriter(@"I:\Utility\BookStore.txt", true))
                {
                    file.WriteLine(emailDetails.Subject);
                }

                this.messageQueue.BeginReceive();
            }
            catch (MessageQueueException qexception)
            {
                Console.WriteLine(qexception);
            }
        }
    }
}
