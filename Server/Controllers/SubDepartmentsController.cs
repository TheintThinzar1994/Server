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
    public class SubDepartmentsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private ISubDepartmentService _subdeptservice;

        public SubDepartmentsController(ApplicationContext context, ISubDepartmentService subdepartservice)
        {
            _context = context;
            _subdeptservice = subdepartservice;
        }

        // GET: api/SubDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubDepartment>>> GetSubDepartments()
        {
            return await _context.SubDepartments.ToListAsync();
        }

        // GET: api/SubDepartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubDepartment>> GetSubDepartment(long? id)
        {
            var subDepartment = await _context.SubDepartments.FindAsync(id);

            if (subDepartment == null)
            {
                return NotFound();
            }

            return subDepartment;
        }

        // PUT: api/SubDepartments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubDepartment(long? id, SubDepartment subDepartment)
        {
            if (id != subDepartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(subDepartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubDepartmentExists(id))
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

        // POST: api/SubDepartments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SubDepartment>> PostSubDepartment(SubDepartment subDepartment)
        {
            _context.SubDepartments.Add(subDepartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubDepartment", new { id = subDepartment.Id }, subDepartment);
        }

        // DELETE: api/SubDepartments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SubDepartment>> DeleteSubDepartment(long? id)
        {
            var subDepartment = await _context.SubDepartments.FindAsync(id);
            if (subDepartment == null)
            {
                return NotFound();
            }

            _context.SubDepartments.Remove(subDepartment);
            await _context.SaveChangesAsync();

            return subDepartment;
        }

        [HttpGet]
        [Route("GetSubDept")]
        public string getSubDepartment(string subdptid)
        {
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<object> getdata = _subdeptservice.getSubDepartment(subdptid);
            returndata.Add(getdata);
            result["status"] = returnstatus;
            result["subdepartment"] = returndata;
            return JsonConvert.SerializeObject(result);

        }
        [HttpPost]
        [Route("SubDepartment")]
        public string InsertSubDepartment(string paramList)
        {
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];
            int Dept_Id = (int)arr["Dept_Id"];           

            //Checking Duplicate Records in Sub Departments by SSM
            var dept_var = _context.SubDepartments.Where(e => e.Name == name && e.Dept_Id==Dept_Id);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Return Conditions Checking for No Duplicate or Not
            if (dept_var.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            else
            {
                //Accepting data comming from Client(PHP)
                var subdptdata = new SubDepartment();
                subdptdata.Name = name;
                subdptdata.Dept_Id = Dept_Id;
                subdptdata.Is_Active = 1;
                subdptdata.ts = DateTime.Now;
                //Inserting data into tables
                SubDepartment subdptresult = _subdeptservice.CreateSubDepartment(subdptdata);
                returndata.Add(subdptresult);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;

            }
            return JsonConvert.SerializeObject(result);

        }
        [HttpPut]
        [Route("UpdateSubDept")]
        public string UpdateSubDepartment(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];
            int id = (int)arr["Id"];
            int Dept_Id = (int)arr["Dept_Id"];

            //Checking Data Have or Not in SubDepartments Table
            var subdepartment = _context.SubDepartments.Where(e => e.Name==name && e.Id != id);

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in SubDepartments Table
            if (subdepartment.Count() <=0)
            {
                // Updating Row data with Client Update Data
                var subdeptdata = new SubDepartment();
                subdeptdata.Id = id;
                subdeptdata.Name = name;
                subdeptdata.Dept_Id = Dept_Id;
                subdeptdata.Is_Active = 1;
                subdeptdata.ts = DateTime.Now;

                SubDepartment subdptresult = _subdeptservice.UpdateSubDepartment(subdeptdata);

                returndata.Add(subdptresult);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            else
            {
                // There is no data to update
                retdata.statuscode = "406";
                retdata.status = "Duplicate Record";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            return JsonConvert.SerializeObject(result);
        }

        [HttpDelete]
        [Route("DeleteSubDept")]
        public string DeleteSubDepartment(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
           // string name = (string)arr["Name"];
            long id = (long)arr["Id"];
            //int Dept_Id = (int)arr["Dept_Id"];

            //Checking Data Have or Not in SubDepartments Table
            var subdepartment = _context.SubDepartments.Where(e => e.Id == id && e.Is_Active == 1);
            
            var employee = _context.Employees.Where(e => e.Sub_Dept_Id == id);

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in SubDepartments Table
            if (subdepartment.Count() > 0)
            {
                if (employee.Count() > 0)
                {
                    // We Cannot delete subdepartment, There is already used in Employee table
                    retdata.statuscode = "400";
                    retdata.status = "Bad Request";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["subdepartment"] = returndata;
                }
                else
                {
                    //There is no Foreign Key Constraint in Employee Table
                    // Deleting Row data with Client Update Data

                    List<SubDepartment> subdptlist = new List<SubDepartment>();
                    var data1 = from s in _context.SubDepartments
                                where s.Id == id && s.Is_Active == 1
                                select s;
                    subdptlist = data1.ToList<SubDepartment>();
                    SubDepartment subdata = new SubDepartment();
                    subdata = subdptlist[0];
                    var subdeptdata = new SubDepartment();
                    //subdeptdata.Id = id;
                    //subdeptdata.Name = subdata.Name;
                    //subdeptdata.Dept_Id = subdata.Dept_Id;
                    subdeptdata.Is_Active = 0;
                    subdeptdata.ts = DateTime.Now;

                    //Deleting Data From Database
                    Boolean subdptresult = _subdeptservice.DeleteSubDepartment(subdeptdata);
                    if (subdptresult)
                    {
                        returndata.Add(subdptresult);
                        retdata.statuscode = "200";
                        retdata.status = "Success";
                        returnstatus.Add(retdata);
                        result["status"] = returnstatus;
                        result["subdepartment"] = returndata;
                    }
                    else
                    {
                        retdata.statuscode = "304";
                        retdata.status = "Not Modified";
                        returnstatus.Add(retdata);
                        result["status"] = returnstatus;
                        result["subdepartment"] = returndata;
                    }
                    
                }
                
            }
            else
            {
                // There is no data to delete
                retdata.statuscode = "304";
                retdata.status = "Not Modified";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            return JsonConvert.SerializeObject(result);
        }
        private bool SubDepartmentExists(long? id)
        {
            return _context.SubDepartments.Any(e => e.Id == id);
        }
    }
}
