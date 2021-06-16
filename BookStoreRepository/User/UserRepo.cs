using BookStoreModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreRepository.User
{
    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration config;
        public UserRepo(IConfiguration configuration)
        {
            this.config = configuration;
        }
        public async Task<string> Login(Login login)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spUserLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("UserName", login.Email.ToLower());
                    command.Parameters.AddWithValue("Password", login.Password);
                    connection.Open();
                    int res = (Int32)command.ExecuteScalar();
                    if (res == 1)
                    {
                        //string jwt = this.GenerateJWTtokens(login.Email);
                        connection.Close();
                        return await Task.Run(() => "jwt");
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
    }
}
