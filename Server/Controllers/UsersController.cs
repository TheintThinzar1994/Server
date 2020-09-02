using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Model;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IUserService _userService;

        public UsersController(ApplicationContext context,IUserService userService )
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
                returndata = _userService.getData(name, password);
                result["status"] =returnstatus ;
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
        public string InsertRole(String paramRole)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramRole);
            string roleName = (string)arr["Name"];
            

            var role = _context.Roles.Where(e => e.Name == roleName);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
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
                Role roles = new Role();
                roles.Name = roleName;
                Role roleresult = _userService.CreateRole(roles);
                returndata.Add(roleresult);
                result["status"] = returnstatus;
                result["role"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpPost]
        [Route("UpdateRole")]
        public string UpdateRole(Role roledata)
        {
            // var user = await _context.Users.FindAsync(id);


            var role = _context.Roles.Where(e => e.Id == roledata.Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (role.Count() > 0)
            {
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                Role roleresult = _userService.UpdateRole(roledata);
                returndata.Add(roleresult);
                result["status"] = returnstatus;
                result["role"] = returndata;               
            }
            else
            {
                retdata.statuscode = "304";
                retdata.status = "Not Modified";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["role"] = returndata;
            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpPost]
        [Route("DeleteRole")]
        public string DeleteRole(Role roledata)
        {
            // var user = await _context.Users.FindAsync(id);


            var role = _context.Roles.Where(e => e.Id == roledata.Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (role.Count() > 0)
            {
                
                Boolean roledelete = _userService.DeleteRole(roledata);
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

        [HttpPost]
        [Route("GetRole")]
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
