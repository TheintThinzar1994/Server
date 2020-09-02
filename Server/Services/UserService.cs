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
        List<object> getData(string username, string password);
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
        public List<object> getData(string username,string password)
        {
            List<object> objlist = new List<object>();
            var query = _context.Users.Where(x => x.User_Name == username)
            .Join(
            _context.MenuRole,
            user => user.Role_ID,
            menurole => menurole.Role.Id,
            (user, menurole) => new
            {
                MenuID = menurole.Menu_Id
            }
            ).Join(
                _context.Menu,
                menurole => menurole.MenuID,
                menu => menu.Id,
                (menurole, menu) => new
                {
                    MenuID = menu.Id,
                    MenuName = menu.Menu_Name,
                    ParentID = menu.Parent_Id,
                    Des = menu.Description,
                    Action = menu.Action,
                    RoutePath = menu.RoutePath,
                    Icon = menu.Icon
                }
                ).ToList();
            objlist = query.ToList<object>();           
            //foreach (var invoice in query)
            //{
                
            //    Console.WriteLine("InvoiceID: {0}, Customer Name: {1} " + "Date: {2} ",
            //        invoice.MenuID, invoice.MenuName, invoice.ParentID,invoice.Des);
            //}
            return objlist;
        }
    }

}
