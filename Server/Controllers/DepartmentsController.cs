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
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IDepartmentService _deptService;

        public DepartmentsController(ApplicationContext context, IDepartmentService deptService)
        {
            _context = context;
            _deptService = deptService;
        }

        // Getting All Department Data
        // GET: api/Departments
        [HttpGet]
        [Route("GetDepart")]
        public string getDepartment(string departmentId)
        {
            // var user = await _context.Users.FindAsync(id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<Department> getdata = _deptService.getDepartment(departmentId);
            returndata.Add(getdata);
            result["status"] = returnstatus;
            result["department"] = returndata;
            return JsonConvert.SerializeObject(result);

        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(long? id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        
        [HttpPut]
        [Route("Updatedepart")]
        public string UpdateDepartment(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            string name = (string)arr["departName"];
            int id = (int)arr["Id"];

            var departData = new Department();
            departData.Id = id;
            departData.Name = name;
            departData.Is_Active = true;
            departData.ts = DateTime.Now;

            var department = _context.Departments.Where(e => e.Id == departData.Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (department.Count() > 0)
            {
                var deptdata = new Department();
                deptdata.Id = id;
                deptdata.Name = name;
                deptdata.Is_Active = true;
                deptdata.ts = DateTime.Now;
                Department departResult = _deptService.updateDept(departData);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                returndata.Add(departResult);
                result["status"] = returnstatus;
                result["department"] = returndata;
            }
            else
            {
                retdata.statuscode = "304";
                retdata.status = "Not Modified";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["department"] = returndata;
            }
            return JsonConvert.SerializeObject(result);

        }

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //Creating New Department
        [HttpPost]
        [Route("Createdept")]
        public string InsertDepartment(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            string deptName = (string)arr["Name"];
            int dept_Id = (int)arr["Dept_Id"];

            var deptData = new Department();
            deptData.Name = deptName;
            deptData.Is_Active = true;
            deptData.ts = DateTime.Now;
            // check duplicate department when creating department
            var department = _context.Departments.Where(e => e.Name == deptName && e.Id == dept_Id);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (department.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Existing Department";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["department"] = returndata;
            }
            else
            {   //come from client data
                var deptdata = new Department();
                deptdata.Name = deptName;
                deptdata.Id = dept_Id;
                deptdata.Is_Active = true;
                deptdata.ts = DateTime.Now;
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                Department roleresult = _deptService.CreateDepartment(deptData);
                returndata.Add(roleresult);
                result["status"] = returnstatus;
                result["role"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }

        // DELETE: api/Departments/5
        [HttpDelete]
        [Route("deleteDepartment")]
        public string deleteDepartment(string paramList)
        {
            // var user = await _context.Users.FindAsync(id);
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];
            int deptid = (int)arr["Id"];

            var deptData = new Department();
            deptData.Id = deptid;
            deptData.Name = name;

            var department = _context.Departments.Where(e => e.Name == name);

            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();

            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            if (department.Count() > 0)
            {
                retdata.statuscode = "400";
                retdata.status = "Bad Request";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["department"] = returndata;
            }
            else
            {
                Boolean delDept = _deptService.delDepartment(deptData);
                if (delDept)
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
                returndata.Add(department);
                result["status"] = returnstatus;
                result["role"] = returndata;
                
            }
            return JsonConvert.SerializeObject(result);
        }
        private bool DepartmentExists(long? id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
