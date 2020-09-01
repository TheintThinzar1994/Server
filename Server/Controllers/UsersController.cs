using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        [Route("CheckUser")]
        public string CheckUser(string user_name)
        {
            // var user = await _context.Users.FindAsync(id);

           
            var user = _context.Users.Where(e => e.User_Name == user_name && e.Password == "susu123456");

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();           
            if (user.Count() > 0)
            {
                retdata.status = "OK";
                returnstatus.Add(retdata);
                returndata = _userService.getData(user_name, "1234");
                result["status"] =returnstatus ;
                result["menu"] = returndata;
                //result["message"] = "Success";
            }
            else
            {
                retdata.status = "Failed";
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

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
