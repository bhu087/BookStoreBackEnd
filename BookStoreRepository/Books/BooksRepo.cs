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
                        Book book = new Book
                        {
                            BookID = (int)reader["BookID"],
                            BookName = reader["BookName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            Price = (int)reader["Price"]
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
                            string subject = "Order Details";
                            string body = string.Empty;
                            string email = string.Empty;
                            foreach (var orders in orderList)
                            {
                                email = orders.Email;
                                body = "Book Name: " + orders.BookName
                                    + "\nBook Author: "+ orders.Author
                                    + "\nBook Price: "+ orders.Price
                                    + ("\nTotal Price: (Rs.{0}*{1}) = {2}\n\n",
                                    orders.Price, orders.Quantity, orders.TotalPrice);
                            }
                            this.MsmqService();
                            this.AddToQueue(email, subject, body);
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


        public async Task<int> WishToCart(int AccountID, int BookID)
        {
            string conn = config["ConnectionString"];
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


        public async Task<IEnumerable<Book>> SortBooks(string sortOrder)
        {
            IEnumerable<Book> sortedBooks;
            try
            {
                IEnumerable<Book> booksList = this.GetAllBooks().Result;
                if (sortOrder.Equals("LowToHigh"))
                {
                    sortedBooks = booksList.OrderBy(c => c.Price);
                }
                else
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
