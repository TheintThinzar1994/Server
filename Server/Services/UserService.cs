using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
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

        List<object> getUser(string userid);
        User InsertUser(User userdata);
        User UpdateUser(User userdata);
        User DeleteUser(User userdata);
        Employee UpdateUserEmployee(User userdata, int emp_id);
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
                    MenuOrder = menu.order,
                    MenuName = menu.Menu_Name,
                    ParentID = menu.Parent_Id,
                    Des = menu.Description,
                    Action = menu.Action,
                    RoutePath = menu.RoutePath,
                    Icon = menu.Icon
                } 
                ).ToList().OrderBy(Menu => Menu.MenuOrder).ToList();
         
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
        public List<object> getUser(string userid)
        {
           
            var data = (from user in _context.Users
                        join role in _context.Roles on user.Role_ID equals role.Id
                        where EF.Functions.Like(user.Id.ToString(), userid) && user.isActive == true
                        select new { user.Id,user.User_Name,user.Password,user.Created_Date,user.Updated_Date,
                            user.Role_ID,RoleName=role.Name});
            
            List<object> userresult = data.ToList<object>();
            return userresult;
        }
        public User InsertUser(User userdata)
        {
            _context.Users.Add(userdata);
            _context.SaveChanges();
            var data = from user in _context.Users
                       where user.User_Name == userdata.User_Name && user.isActive == true
                       select user;
            List<User> users = data.ToList<User>();
            return users[0];
        }
        public Employee UpdateUserEmployee(User userdata,int emp_id)
        {            
            var data = from emp in _context.Employees
                       where emp.Id == emp_id && emp.isActive == true 
                       select emp;
            List<Employee> empdata = data.ToList<Employee>();
            if (empdata.Count() > 0)
            {
                empdata[0].User_Id = (long)userdata.Id;
                empdata[0].ts = DateTime.Now;
                _context.Employees.Update(empdata[0]);
                _context.SaveChanges();
                return empdata[0];

            }
            else
            {
                Employee e = new Employee();
                return e;
            }
            
        }
        public User UpdateUser(User userdata)
        {
            var updateuser = new User()
            {
                Id = userdata.Id,
                User_Name = userdata.User_Name,
                Password = userdata.Password,
                isActive = true,
                Updated_Date=DateTime.Now,
                ts=DateTime.Now,
                Role_ID=userdata.Role_ID

            };
            _context.Users.Update(updateuser);
            _context.SaveChanges();
             //comment by snh
            //var userdata1 = (User) _context.Users.Where(e => e.Id == userdata.Id);
            return updateuser;
        }
        public User DeleteUser(User userdata)
        {
            //var updateuser = new User()
            //{
            //    Id = userdata.Id,
            //    User_Name = userdata.User_Name,
            //    Password = userdata.Password,
            //    isActive = false,
            //    Updated_Date = DateTime.Now,
            //    ts = DateTime.Now,
            //    Role_ID = userdata.Role_ID

            //};
            _context.Users.Update(userdata);
            _context.SaveChanges();
            //User userdata1 = (User)_context.Users.Where(e => e.Id == userdata.Id);
            var data = from user in _context.Users
                       where user.Id == userdata.Id && user.isActive == false
                       select user;
            List<User> users = data.ToList<User>();
            return users[0];
        }
    }

}
