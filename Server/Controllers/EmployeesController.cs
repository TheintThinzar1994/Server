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
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IEmployeeService _employeeservice;
        private IUserService _userservice;
        public EmployeesController(ApplicationContext context,IEmployeeService employeeService,IUserService userservice)
        {
            _context = context;
            _employeeservice = employeeService;
            _userservice = userservice;
           
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long? id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long? id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(long? id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }
        [HttpGet]
        [Route("GetEmployee")]
        public string getEmployees(string empid)
        {
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            List<object> getdata = _employeeservice.getEmployee(empid);
            returndata.Add(getdata);
            result["status"] = returnstatus;
            result["employee"] = returndata;
            return JsonConvert.SerializeObject(result);

        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public string UpdateEmployee(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int Id = (int)arr["Id"];
            string name = (string)arr["Name"];
            int sub_dept_id = (int)arr["Sub_Dept_Id"];
            int dept_id = (int)arr["Dept_Id"];
            string user_name = (string)arr["User_Name"];
            string password = (string)arr["Password"];
            int role_id = (int)arr["Role_Id"];
            string address = (string)arr["Address"];
            string email = (string)arr["email"];
            string phone = (string)arr["phone"];
            string photoname = (string)arr["photoname"];
            int user_id = (int)arr["user_id"];


            //Checking Duplicate Records in Sub Departments by SSM
            //add new id check by snh
            var emp_var = _context.Employees.Where(e => e.User_Name == name && e.Dept_Id == dept_id && e.Sub_Dept_Id == sub_dept_id && e.Id != Id && e.isActive == true);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnuser = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Checking Data Already exists in Database or Not
            if (emp_var.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "The Update Employee is Already Exist, Try Again";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["employee"] = returndata;
                result["user"] = returnuser;
            }
            else
            {
                // Checking Data Have in Database or Not (Before Updating)
                emp_var = _context.Employees.Where(e =>  e.Id==Id);
                // There is No Data To Update
                if(emp_var.Count()<=0)
                {
                    retdata.statuscode = "304";
                    retdata.status = "There is no Data To Update";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["employee"] = returndata;
                    result["user"] = returnuser;
                }
                // Updating Data into Database
                else
                {
                    // Checking Update User Name Already Have or Not
                    var user = from u in _context.Users
                               where u.User_Name == user_name && u.isActive == true && u.Id != user_id
                               select u;
                    if (user.Count() > 0)
                    {
                        retdata.statuscode = "406";
                        retdata.status = "The Update User Name is Already Exist, Try Again";
                        returnstatus.Add(retdata);
                        result["status"] = returnstatus;
                        result["employee"] = returndata;
                        result["user"] = returnuser;
                    }
                    else
                    {
                        List<Employee> empdptlist = new List<Employee>();
                        var data1 = from e in _context.Employees
                                    where e.Id == Id && e.isActive == true
                                    select e;
                        empdptlist = data1.ToList<Employee>();
                        Employee empdata = new Employee();
                        empdata = empdptlist[0];
                        empdata.Id = Id;
                        empdata.User_Name = name;
                        empdata.Sub_Dept_Id = sub_dept_id;
                        empdata.Dept_Id = dept_id;
                        empdata.User_Id = empdata.User_Id;
                        empdata.Address = address;
                        empdata.Email = email;
                        empdata.Phone = phone;
                        empdata.PhotoName = photoname;
                        empdata.isActive = true;
                        empdata.Created_Date = empdata.Created_Date;
                        empdata.End_Date = empdata.End_Date;
                        empdata.ts = DateTime.Now;
                        //Inserting data into tables
                        Employee empresult = _employeeservice.UpdateEmployee(empdata);
                        var data2 = from u in _context.Users
                                    where u.Id == empdata.User_Id && u.isActive == true
                                    select u;
                        if (data2.Count() > 0)
                        {
                            List<User> userdata = new List<User>();
                            userdata = data2.ToList<User>();
                            userdata[0].Id = empdata.User_Id;
                            userdata[0].User_Name = user_name;
                            userdata[0].Password = password;
                            userdata[0].Role_ID = role_id;
                            userdata[0].isActive = true;
                            userdata[0].ts = DateTime.Now;
                            User userdata1 = _userservice.UpdateUser(userdata[0]);
                            returnuser.Add(userdata1);
                        }
                        returndata.Add(empresult);
                        retdata.statuscode = "200";
                        retdata.status = "Success";
                        returnstatus.Add(retdata);
                        result["status"] = returnstatus;
                        result["employee"] = returndata;
                        result["user"] = returnuser;
                    }
                    
                }   
            }
            return JsonConvert.SerializeObject(result);

        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public string DeleteEmployee(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int Id = (int)arr["Id"];
            //string name = (string)arr["Name"];
            //int sub_dept_id = (int)arr["Sub_Dept_Id"];
            //int dept_id = (int)arr["Dept_Id"];
            //int user_id = (int)arr["User_Id"];
            //string address = (string)arr["Address"];
            //string email = (string)arr["email"];
            //int phone = (int)arr["phone"];
            //string photoname = (string)arr["photoname"];


            //Checking Duplicate Records in Sub Departments by SSM
            var emp_var = _context.Employees.Where(e =>  e.Id==Id && e.isActive==true);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnuser = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Checking Data Have or not in database to delete
            if (emp_var.Count() < 0)
            {
                retdata.statuscode = "304";
                retdata.status = "There is no Employee Data To Delete";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["employee"] = returndata;
            }
            else
            {
                //Deleting data from Database
                List<Employee> emplist = new List<Employee>();
                var data1 = from e in _context.Employees
                            where e.Id == Id && e.isActive == true
                            select e;
                emplist = data1.ToList<Employee>();
                Employee empdata = new Employee();
                empdata = emplist[0];

                empdata.Id = Id;
                empdata.User_Name = empdata.User_Name;
                empdata.Sub_Dept_Id = empdata.Sub_Dept_Id;
                empdata.Dept_Id = empdata.Dept_Id;
                empdata.User_Id = empdata.User_Id;
                empdata.Address = empdata.Address;
                empdata.Email = empdata.Email;
                empdata.Phone = empdata.Phone;
                empdata.PhotoName = empdata.PhotoName;
                empdata.isActive = false;
                empdata.Created_Date = empdata.Created_Date;
                empdata.End_Date = empdata.End_Date;
                empdata.ts = DateTime.Now;
                //Updateing data from tables
                Employee empresult = _employeeservice.UpdateEmployee(empdata);

                User userdata = new User();
                var data2 = from u in _context.Users
                             where u.Id == empdata.User_Id && u.isActive == true
                             select u;
                if (data2.Count() > 0)
                {
                    List<User> ulist = data2.ToList<User>();
                    userdata = ulist[0];
                    userdata.isActive = false;
                    userdata.ts = DateTime.Now;
                    User u = _userservice.UpdateUser(userdata);
                    returnuser.Add(u);

                }

                returndata.Add(empresult);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["employee"] = returndata;
                result["user"] = returnuser;
            }
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        [Route("Employee")]
        public string InsertEmployee(string paramList)
        {
            var arr = JObject.Parse(paramList);
            string name = (string)arr["Name"];
            int sub_dept_id = (int)arr["Sub_Dept_Id"];
            int dept_id = (int)arr["Dept_Id"];
            string user_name = (string)arr["User_Name"];
            string password = (string)arr["password"];
            int role_id = (int)arr["role_id"];
            string address = (string)arr["Address"];
            string email = (string)arr["email"];
            string phone = (string)arr["phone"];
            string photoname = (string)arr["photoname"];


            //Checking Duplicate Records in Sub Departments by SSM
            var dept_var = _context.Employees.Where(e => e.User_Name == name && e.Dept_Id == dept_id && e.Sub_Dept_Id == sub_dept_id);

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> retrunuser = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            // Return Conditions Checking for No Duplicate or Not
            if (dept_var.Count() > 0)
            {
                retdata.statuscode = "406";
                retdata.status = "The Insert Employee Name is Already Exist, Try Again";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["employee"] = returndata;
                result["user"] = retrunuser;
            }
            else
            {
                //Checking User Data Already have or not
                var data = from user in _context.Users
                           where user.User_Name == user_name && user.isActive == true
                           select user;
                if (data.Count() > 0)
                {
                    retdata.statuscode = "406";
                    retdata.status = "The Insert User Name is Already Exist, Try Again";
                    returnstatus.Add(retdata);
                    result["status"] = returnstatus;
                    result["employee"] = returndata;
                    result["user"] = retrunuser;
                }

                else
                {
                    var empdata = new Employee();
                    empdata.User_Name = name;
                    empdata.Sub_Dept_Id = sub_dept_id;
                    empdata.Dept_Id = dept_id;
                    //empdata.User_Id = user_id;
                    empdata.Address = address;
                    empdata.Email = email;
                    empdata.Phone = phone;
                    empdata.PhotoName = photoname;
                    empdata.Created_Date = DateTime.Now;
                    empdata.End_Date = DateTime.Now;
                    empdata.isActive = true;
                    empdata.ts = DateTime.Now;
                    //Inserting data into tables
                    
                    User user = new User();
                    user.User_Name = user_name;
                    user.Password = password;
                    user.Role_ID = role_id;
                    user.isActive = true;
                    user.ts = DateTime.Now;
                    User userdata = _userservice.InsertUser(user);
                    if (userdata != null)
                    {
                        //Inserting Empoyee Data
                        empdata.User_Id =(long) userdata.Id;
                        Boolean empresult = _employeeservice.CreateEmployee(empdata);
                        if (empresult)
                        {
                            retrunuser.Add(userdata);
                            returndata.Add(empdata);
                            retdata.statuscode = "200";
                            retdata.status = "Success";
                            returnstatus.Add(retdata);
                            result["status"] = returnstatus;
                            result["employee"] = returndata;
                            result["user"] = retrunuser;
                        }
                        else
                        {
                            user.isActive = false;
                            User userre = _userservice.DeleteUser(user);
                            retdata.statuscode = "304";
                            retdata.status = "Failed To Insert Employee Data, Please Try Again?";
                            returnstatus.Add(retdata);
                            result["status"] = returnstatus;
                            result["employee"] = returndata;
                            result["user"] = retrunuser;
                        }

                    }
                    else
                    {                        
                        retdata.statuscode = "304";
                        retdata.status = "Failed To Insert User Data, Please Try Again?";
                        returnstatus.Add(retdata);
                        result["status"] = returnstatus;
                        result["employee"] = returndata;
                        result["user"] = retrunuser;
                    }
                   
                    
                }


                

            }
            return JsonConvert.SerializeObject(result);

        }





        private bool EmployeeExists(long? id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
