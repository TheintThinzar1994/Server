using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IEmployeeService
    {
        List<object> getEmployee(string empid);
        Employee CreateEmployee(Employee empdata);
        Employee UpdateEmployee(Employee empupdate);
       List<object> getEmployeeByUser(string name);
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

        public List<object> getEmployeeByUser(string name)
        {
            
            var data = from users in _context.Users
                     where users.User_Name == name
                       select users;
            List<User> userlist = data.ToList<User>();
            int user_id = (int)userlist[0].Id;
            var empdata = from employee in _context.Employees
                          join dept in _context.Departments on employee.Dept_Id equals dept.Id
                          join sub in _context.SubDepartments on employee.Sub_Dept_Id equals sub.Id
                          where employee.User_Id == user_id && employee.isActive == true
                          select new
                          {
                              Emp_Id = employee.Id,
                              Emp_Name = employee.User_Name,
                              Emp_PhotoName = employee.PhotoName,
                              Dept_Id = dept.Id,
                              Dept_Name = dept.Name,
                              Sub_dep_ID = sub.Id,
                              Sub_dep_Name = sub.Name
                          };
            List<object> employees = empdata.ToList<object>();
            return employees;

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
            //edit isActive check by snh
            var data = from employee in _context.Employees
                       where employee.Id == empupdate.Id && employee.isActive == empupdate.isActive
                       select employee;
            List<Employee> employees = data.ToList<Employee>();
            return employees[0];
        }
    }
}
