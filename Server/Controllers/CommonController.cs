using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Model;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private ISubDepartmentService _subdeptservice;
        private IDepartmentService _deptservice;
        private IUserService _userservice;
        private ICommomService _commonservice;
        public CommonController(ApplicationContext context,ISubDepartmentService subdeptservice,IDepartmentService deptservice,IUserService userservice, ICommomService commomservice)
        {
            _context = context;
            _subdeptservice = subdeptservice;
            _deptservice = deptservice;
            _userservice = userservice;
            _commonservice = commomservice;
        }

        // GET: api/Common
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Common/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long? id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Common/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long? id, User user)
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

        // POST: api/Common
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Common/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long? id)
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
        [HttpGet]
        [Route("GetCommonData")]
        public string getCommonData()
        {
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<object> subdptlist = _subdeptservice.getSubDepartment("%");
            List<Department> dptlist = _deptservice.getDepartment("%");
            List<object> userlist = _commonservice.getUserForEmployeeSetup("%");
            returndata.Add(dptlist);
            result["status"] = returnstatus;
            result["department"] = returndata;
            result["subdepartment"] = subdptlist;
            result["user"] = userlist;
            return JsonConvert.SerializeObject(result);

        }
        [HttpGet]
        [Route("GetCommonDataEdit")]
        public string getCommonDataForEdit(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int emp_id = (int)arr["emp_id"];
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<object> subdptlist = _subdeptservice.getSubDepartment("%");
            List<Department> dptlist = _deptservice.getDepartment("%");
            List<object> userlist = _commonservice.getUserForEmployeeEdit("%",emp_id);
            returndata.Add(dptlist);
            result["status"] = returnstatus;
            result["department"] = returndata;
            result["subdepartment"] = subdptlist;
            result["user"] = userlist;
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        [Route("GetUserSetupCommon")]
        public string getCommonUserSetup()
        {
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            //List<object> subdptlist = _subdeptservice.getSubDepartment("%");
            //List<Department> dptlist = _deptservice.getDepartment("%");
            List<object> emplist = _commonservice.getEmployeeForUserSetup("%");
            returndata.Add(emplist);
            result["status"] = returnstatus;
            result["employee"] = returndata;
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        [Route("GetUserEditCommon")]
        public string getCommonUserEdit(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int user_id = (int)arr["user_id"];
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            //List<object> subdptlist = _subdeptservice.getSubDepartment("%");
            //List<Department> dptlist = _deptservice.getDepartment("%");
            List<object> emplist = _commonservice.getEmployeeForUserEdit(user_id,"%");
            returndata.Add(emplist);
            result["status"] = returnstatus;
            result["employee"] = returndata;
            return JsonConvert.SerializeObject(result);

        }

        private bool UserExists(long? id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
