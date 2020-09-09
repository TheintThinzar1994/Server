using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ICommomService
    {
       List<object> getUserForEmployeeSetup(string userid);
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
                        join emp in _context.Employees on user.Id !equals emp.User_Id
                        join role in _context.Roles on user.Role_ID equals role.Id
                        where EF.Functions.Like(user.Id.ToString(), userid) && user.isActive == true    
                        && user.Id !=emp.User_Id
                        select new
                        {
                            user.Id,
                            user.User_Name,
                            user.Password,
                            user.Created_Date,
                            user.Updated_Date,
                            user.Role_ID,
                            RoleName = role.Name
                        });
            

            List<object> userresult = data.ToList<object>();
            return userresult;
        }
    }
}
