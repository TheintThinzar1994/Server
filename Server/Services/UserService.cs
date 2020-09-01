using Server.Helpers;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        //User GetById(int id);
        User Create(string username,string password,int role_id);
        //void Update(User user, string password = null);
        // void Delete(int id);
    }
    public class UserService : IUserService
    {
        private ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public User Create(string username, string password, int role_id)
        {
            User user = new User();
            user.User_Name = username;
            user.Password = password;
            user.Role_ID = role_id;

            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.User_Name == username))
                throw new AppException("Username \"" + username + "\" is already taken");
            
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;


        }
       
    }

}
