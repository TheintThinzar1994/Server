using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ICommomService
    {
       List<object> getUserForEmployeeSetup(string userid);
       List<object> getUserForEmployeeEdit(string userid, int empid);
    }
    public class CommonService : ICommomService
    {
        private ApplicationContext _context;
        public CommonService(ApplicationContext context)
        {
            _context = context;
        }
        public List<object> getUserForEmployeeSetup(string userid)
        {
            var userdata = from emp in _context.Employees
                        join user in _context.Users on emp.User_Id equals user.Id
                        select user;
            List<User> userlist = new List<User>();
            userlist = userdata.ToList<User>();

            var data = (from user in _context.Users
                        join emp in _context.Employees on user.Id equals emp.User_Id
                        join role in _context.Roles on user.Role_ID equals role.Id
                        where EF.Functions.Like(user.Id.ToString(), userid) && user.isActive == true && emp.isActive == true
                        select user
                       ).Distinct()
                       ;
            var udata = from user in _context.Users
                        where user.isActive == true
                        select user;

            udata = udata.Except(data);


            List<object> userresult = udata.ToList<object>();
            return userresult;
        }
        public List<object> getUserForEmployeeEdit(string userid,int empid)
        {
            var userdata = from emp in _context.Employees.Where(e=>e.Id==empid)
                           join user in _context.Users on emp.User_Id equals user.Id
                           where  emp.isActive==true && user.isActive==true
                           select user;
            List<User> userlist = new List<User>();
            userlist = userdata.ToList<User>();

            var data = (from user in _context.Users
                        join emp in _context.Employees.Where(e=>e.Id!=empid) on user.Id equals emp.User_Id 
                        join role in _context.Roles on user.Role_ID equals role.Id
                        where EF.Functions.Like(user.Id.ToString(), userid) && user.isActive == true && emp.isActive==true
                        select user  
                        ).Distinct()
                        ;
            var udata = from user in _context.Users
                        where user.isActive == true
                        select user;

            udata = udata.Except(data);


            List<object> userresult = udata.ToList<object>();
            return userresult;
        }
    }
}
