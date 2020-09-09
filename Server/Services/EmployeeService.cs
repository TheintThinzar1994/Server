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
        List<object> getEmployee(string subdptid);
        Employee CreateEmployee(Employee empdata);
        Employee UpdateEmployee(Employee empupdate);
       List<object> getEmployeeByUser(string user_name);
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

        public List<object> getEmployeeByUser(string user_name)
        {
            List<object> emplist = new List<object>();
            var empdata = (from user in _context.Users
                        join emp in _context.Employees on user.Id equals emp.User_Id
                        join dept in _context.Departments on emp.Dept_Id equals dept.Id
                        join sub in _context.SubDepartments on emp.Sub_Dept_Id equals sub.Id
                        join role in _context.Roles on user.Role_ID equals role.Id
                        where EF.Functions.Like(user.User_Name.ToString(), user_name) && user.isActive == true                        
                        select new
                        {
                            user.Id,
                            user.User_Name,
                            user.Password,                            
                            user.Updated_Date,
                            user.Role_ID,
                            RoleName = role.Name,
                            Emp_Id = emp.Id,
                            Emp_Name = emp.User_Name,
                            emp.Address,
                            emp.Email,
                            emp.Phone,
                            emp.PhotoName,
                            emp.Created_Date,
                            emp.End_Date,
                            Dept_Id = dept.Id,
                            Dept_Name = dept.Name,
                            Sub_dep_ID = sub.Id,
                            Sub_dep_Name = sub.Name
                        });
            //var empdata = from e in _context.Employees
            //              join d in _context.Departments on e.Dept_Id equals d.Id
            //              join sd in _context.SubDepartments on e.Sub_Dept_Id equals sd.Id
            //              join u in _context.Users on e.User_Id equals u.Id
            //              where EF.Functions.Like(u.User_Name.ToString(), user_name) && e.isActive == true
            //              select u;
                          //select new
                          //{
                          //    Emp_Id = e.Id,
                          //    Emp_Name = e.User_Name,
                          //    Sub_Dept_Id = sd.Id,
                          //    Sub_Dept_Name = sd.Name,
                          //    Dept_Id = d.Id,
                          //    Dept_Name = d.Name,
                          //    User_Id = u.Id,
                          //    User_Name = u.User_Name,
                          //    e.Address,
                          //    e.Email,
                          //    e.PhotoName,
                          //    e.Created_Date,
                          //    e.End_Date
                          //};
            try
            {
                emplist = empdata.ToList<object>();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            //edit isActive check by snh
            var data = from employee in _context.Employees
                       where employee.Id == empupdate.Id && employee.isActive == empupdate.isActive
                       select employee;
            List<Employee> employees = data.ToList<Employee>();
            return employees[0];
        }
    }
}
