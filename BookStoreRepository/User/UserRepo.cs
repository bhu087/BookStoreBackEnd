using BookStoreModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                using (SqlCommand command = new SqlCommand("spLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("Email", login.Email.ToLower());
                    command.Parameters.AddWithValue("Password", login.Password);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        string jwt = string.Empty;
                        while (reader.Read())
                        {
                            int Id = (int)reader["AccountID"];
                            jwt = this.GenerateJWTtokens(login.Email, Id);
                        }
                        connection.Close();
                        return await Task.Run(() => jwt);
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

        public async Task<UserDetails> AddUser(UserDetails user)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("Name", user.Name);
                    command.Parameters.AddWithValue("Email", user.Email);
                    command.Parameters.AddWithValue("Mobile", user.Mobile);
                    command.Parameters.AddWithValue("Password", user.Password);
                    command.Parameters.AddWithValue("HolderState", 1);
                    connection.Open();
                    int res = (Int32)command.ExecuteScalar();
                    if (res == 1)
                    {
                        connection.Close();
                        return await Task.Run(() => user);
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

        public async Task<UserDetails> GetUser(int userID)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", userID);
                    UserDetails userDetails = new UserDetails();
                    connection.Open();
                    var res = command.ExecuteReader();
                    if (res != null)
                    {
                        while (res.Read())
                        {
                            userDetails.AccountId = (int)res["AccountID"];
                            userDetails.Name = res["Name"].ToString();
                        }
                        connection.Close();
                        return await Task.Run(() => userDetails);
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

        public async Task<IEnumerable<string>> GetAllAddress(int userID)
        {
            string conn = config["ConnectionString"];
            List<string> addressList = new List<string>();
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spGetAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", userID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        string address = reader["Address"].ToString();
                        addressList.Add(address);
                    }
                    connection.Close();
                    if (addressList.Count == 0)
                    {
                        return null;
                    }
                    return await Task.Run(() => addressList);
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

        public async Task<int> AddNewAddress(int userID, string address)
        {
            string conn = config["ConnectionString"];
            SqlConnection connection = new SqlConnection(conn);
            try
            {
                using (SqlCommand command = new SqlCommand("spAddNewAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("AccountID", userID);
                    command.Parameters.AddWithValue("Address", address);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return await Task.Run(() => result);
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

        public string GenerateJWTtokens(string userEmail, int Id)
        {
            string key = this.config["Jwt:Key"];
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "User"),
                            new Claim("Email", userEmail),
                            new Claim("Id", Id.ToString())
                        }),
                    Expires = DateTime.Now.AddDays(Convert.ToDouble(this.config["Jwt:JwtExpireDays"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var securityTokenHandler = new JwtSecurityTokenHandler();
                var securityToken = securityTokenHandler.CreateToken(tokenDescriptor);
                var token = securityTokenHandler.WriteToken(securityToken);
                return token;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
