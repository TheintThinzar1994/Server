using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IEmployeeService
    {
        List<object> getEmployee(string subdptid);
        Employee CreateEmployee(Employee empdata);
        Employee UpdateEmployee(Employee empupdate);
    }
    public class EmployeeService : IEmployeeService
    {
        private ApplicationContext _context;

        public EmployeeService(ApplicationContext context)
        {
            _context = context;
        }
        public List<object> getEmployee(string empid)
        {
            List<object> emplist = new List<object>();            
            var data1 = from e in _context.Employees
                        join d in _context.Departments on e.Dept_Id equals d.Id
                        join sd in _context.SubDepartments on e.Sub_Dept_Id equals sd.Id
                        join u in _context.Users.DefaultIfEmpty() on e.User_Id equals u.Id
                        where EF.Functions.Like(e.Id.ToString(), empid) && e.isActive == true
                        select new { Emp_Id=e.Id,Emp_Name=e.User_Name,Sub_Dept_Id=sd.Id,Sub_Dept_Name=sd.Name,
                        Dept_Id=d.Id,Dept_Name=d.Name,User_Id=u.Id,User_Name=u.User_Name,e.Address,e.Email,e.Phone,e.PhotoName,
                        e.Created_Date,e.End_Date
                        };
            emplist = data1.ToList<object>();
            return emplist;
        }
        public Employee CreateEmployee(Employee empdata)
        {
            _context.Employees.Add(empdata);
            _context.SaveChanges();
            return empdata;
        }
        public Employee UpdateEmployee(Employee empupdate)
        {            
            _context.Employees.Update(empupdate);
            _context.SaveChanges();
            var data = from employee in _context.Employees
                       where employee.Id == empupdate.Id && empupdate.isActive == true
                       select employee;
            List<Employee> employees = data.ToList<Employee>();
            return employees[0];
        }
    }
}
