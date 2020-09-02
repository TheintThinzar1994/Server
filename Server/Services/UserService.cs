using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        //User GetById(int id);
        User Create(string username,string password,int role_id);
        Role CreateRole(Role roledata);
        List<object> getData(string username, string password);
        Role UpdateRole(Role roledata);
        Boolean DeleteRole(Role roledata);

        List<Role> getRole(string roleid);
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
        //ToCreate User Data
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
        //Getting User Login Information 01/09/2020
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
        public Role CreateRole(Role roledata)
        {
            _context.Roles.Add(roledata);
            _context.SaveChanges();
            return roledata;
        }
        public Role UpdateRole(Role roledata)
        {            
            var updaterole = new Role()
            {
                Id = roledata.Id,
                Name = roledata.Name,
                isActive = true,
                ts = DateTime.Now

            };
            _context.Roles.Update(updaterole);
            _context.SaveChanges();
            return roledata;
        }
        public Boolean DeleteRole(Role roledata)
        {
            var updaterole = new Role()
            {
                Id = roledata.Id,
                Name = roledata.Name,
                isActive = false,
                ts = DateTime.Now
            };
            try
            {
                _context.Roles.Update(updaterole);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Record does not exist in the database");
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
        public List<Role> getRole(string roleid)
        {            
            var data = from s in _context.Roles
                        where EF.Functions.Like(s.Id.ToString(), roleid) && s.isActive==true
                        select s;
            List<Role> roledata = data.ToList<Role>();
            return roledata;
        }
    }

}
