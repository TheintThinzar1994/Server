using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Server.Model;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IUserService _userService;
        private IEmployeeService _empService;

        public UsersController(ApplicationContext context, IUserService userService,IEmployeeService empService)
        {
            _context = context;
            _userService = userService;
            _empService = empService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(long id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}




        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpGet]
        [Route("GetUser")]
        public string GetUser(string userid)
        {
            // var user = await _context.Users.FindAsync(id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<object> getdata = _userService.getUser(userid);
            returndata.Add(getdata);
            result["status"] = returnstatus;
            result["user"] = returndata;
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        [Route("User")]
        public string InsertUser(string paramList)
        {        
            var arr = JObject.Parse(paramList);
            string name = (string)arr["User_Name"];
            string password = (string)arr["password"];
            int Role_Id = (int)arr["role_id"];
            int Emp_Id = (int)arr["emp_id"];
           

            //Checking Duplicate Records in Users by SSM
            var user = _context.Users.Where(e => e.User_Name == name);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Return Conditions Checking for No Duplicate or Not
            if (user.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["user"] = returndata;
            }
            else
            {
                var userData = new User();
                userData.User_Name = name;
                userData.Password = password;
                userData.isActive = true;
                userData.Created_Date = DateTime.Now;
                userData.Updated_Date = DateTime.Now;
                userData.ts = DateTime.Now;
                userData.Role_ID = Role_Id;
                
                User userresult = _userService.InsertUser(userData);


                List<Employee> emplist = new List<Employee>();
                var data1 = from s in _context.Employees
                            where s.Id == Emp_Id && s.isActive == true
                            select s;
                emplist = data1.ToList<Employee>();
                Employee empdata = new Employee();
                if (emplist.Count() > 0)
                {
                    empdata = emplist[0];
                    empdata.User_Id = (long)userresult.Id;
                    Employee empresult = _empService.UpdateEmployee(empdata);
                }

                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                returndata.Add(userresult);
                result["status"] = returnstatus;
                result["user"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        [Route("UpdateUser")]
        public string UpdateUser(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int Id = (int)arr["Id"];
            string name = (string)arr["User_Name"];
            string password = (string)arr["password"];
            int Role_Id = (int)arr["role_id"];


            //Checking Duplicate Records in Users by SSM
            var user = _context.Users.Where(e => e.User_Name == name && e.isActive==true);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Return Conditions Checking for No Duplicate before update new User Name
            if (user.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["user"] = returndata;
            }
            else
            {
                var userData = new User();
                userData.User_Name = name;
                userData.Password = password;
                userData.isActive = true;
                userData.Updated_Date = DateTime.Now;
                userData.ts = DateTime.Now;
                userData.Role_ID = Role_Id;

                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                User userresult = _userService.UpdateUser(userData);
                returndata.Add(userresult);
                result["status"] = returnstatus;
                result["user"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpDelete]
        [Route("DeleteUser")]
        public string DeleteUser(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int Id = (int)arr["Id"];
            //string name = (string)arr["User_Name"];
            //string password = (string)arr["password"];
            //int Role_Id = (int)arr["role_id"];


            //Checking Duplicate Records in Users by SSM
            var user = _context.Users.Where(e => e.Id == Id && e.isActive==true);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Data Have in Database or Not to delete
            if (user.Count() > 0)
            { 
                // Checking User Already Assigned in Employee or Not
                var employee = _context.Employees.Where(e => e.User_Id == Id);
                if (employee.Count() < 0)
                {

                    List<User> userlist = new List<User>();
                    var data1 = from u in _context.Users
                                where u.Id == Id && u.isActive == true
                                select u;
                    userlist = data1.ToList<User>();
                    User userData = new User();
                    userData = userlist[0];
                    userData.Id = userData.Id;
                    userData.User_Name = userData.User_Name;
                    userData.Password = userData.Password;
                    userData.isActive = false;
                    userData.Updated_Date = DateTime.Now;
                    userData.ts = DateTime.Now;
                    userData.Role_ID = userData.Role_ID;

                    User userresult = _userService.DeleteUser(userData);
                    returndata.Add(userresult);
                    retdata.statuscode = "200";
                    retdata.status = "Success";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["user"] = returndata;
                }
                else
                {
                    retdata.statuscode = "304";
                    retdata.status = "Data Have already used in employee";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["user"] = returndata;
                }
                
                
            }
            else
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["user"] = returndata;
            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("Check")]
        public string CheckUser(String paramsList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramsList);
            string name = (string)arr["user_name"];
            string password = (string)arr["password"];


            var user = _context.Users.Where(e => e.User_Name == name && e.Password == password);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (user.Count() > 0)
            {
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                returndata = _userService.getData(name, "1234");
                result["status"] = returnstatus;
                result["menu"] = returndata;
                //result["message"] = "Success";
            }
            else
            {
                retdata.statuscode = "401";
                retdata.status = "Unauthorized";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                //result["message"] = "Failed";
                result["menu"] = returndata;

            }
            return JsonConvert.SerializeObject(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            //try
            //{
            //    // create user
            //    //_userService.Create();
            //    return Ok();
            //}
            //catch (AppException ex)
            //{
            //    // return error message if there was an exception

            //}
            return BadRequest();
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        //User Role CRUD

        [HttpPost]
        [Route("Role")]
        public string InsertRole(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];

            var roleData = new Role();
            roleData.Name = name;
            roleData.isActive = true;
            roleData.ts = DateTime.Now;

            //Checking Duplicate Records in Role by SSM
            var role = _context.Roles.Where(e => e.Name == name);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Return Conditions Checking for No Duplicate or Not
            if (role.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["role"] = returndata;
            }
            else
            {
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                Role roleresult = _userService.CreateRole(roleData);
                returndata.Add(roleresult);
                result["status"] = returnstatus;
                result["role"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpPost]
        [Route("UpdateRole")]
        public string UpdateRole(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];
            int id = (int)arr["Id"];

            var roleData = new Role();
            roleData.Id = id;
            roleData.Name = name;
            roleData.isActive = true;
            roleData.ts = DateTime.Now;

            //Checking Duplicate Records in Sub Departments by SSM
            var duprole = _context.Roles.Where(e => e.Name == name && e.isActive == true);

            //Check Data already have in database or not 
            var role = _context.Roles.Where(e => e.Id == roleData.Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Duplicate Record or Not
            if (duprole.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Data";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["role"] = returndata;
            }
            else
            {
                // Checking Data Have in Database or Not (Before Updating)
                if (role.Count() > 0)
                {
                    retdata.statuscode = "200";
                    retdata.status = "Success";
                    returnstatus.Add(retdata);
                    Role roleresult = _userService.UpdateRole(roleData);
                    returndata.Add(roleresult);
                    result["status"] = returnstatus;
                    result["role"] = returndata;
                }
                else
                {
                    retdata.statuscode = "304";
                    retdata.status = "No Data To Modify";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["role"] = returndata;
                }
            }
            
            return JsonConvert.SerializeObject(result);

        }
        [HttpPost]
        [Route("DeleteRole")]
        public string DeleteRole(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            int id = (int)arr["Id"];

            var roleData = new Role();
            roleData.Id = id;

            var role = _context.Roles.Where(e => e.Id == roleData.Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (role.Count() > 0)
            {

                Boolean roledelete = _userService.DeleteRole(roleData);
                if (roledelete)
                {
                    retdata.statuscode = "200";
                    retdata.status = "Success";
                    returnstatus.Add(retdata);
                }
                else
                {
                    retdata.statuscode = "304";
                    retdata.status = "Fail";
                    returnstatus.Add(retdata);
                }
                returndata.Add(role);
                result["status"] = returnstatus;
                result["role"] = returndata;
            }
            else
            {
                retdata.statuscode = "304";
                retdata.status = "No Data To Delete";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["role"] = returndata;
            }
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        [Route("Getrole")]
        public string GetRole(string roleid)
        {
            // var user = await _context.Users.FindAsync(id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<Role> getdata = _userService.getRole(roleid);
            returndata.Add(getdata);
            result["status"] = returnstatus;
            result["role"] = returndata;
            return JsonConvert.SerializeObject(result);

        }
        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


    }
}
