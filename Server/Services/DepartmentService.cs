using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IDepartmentService
    {
       // User Create(string username, string password, int role_id);
        Department CreateDepartment(Department deptData);
        List<Department> getDepartment(string departmentId);
        //List<object> getData(string username, string password);
        Department updateDept(Department departData);
        Boolean delDepartment(Department deptData);
    }
    public class DepartmentService : IDepartmentService
    {
        private ApplicationContext _context;

        public DepartmentService(ApplicationContext context)
        {
            _context = context;
        }
        public Department CreateDepartment(Department deptData)
        {
            _context.Departments.Add(deptData);
            _context.SaveChanges();
            return deptData;
        }
        public List<Department> getDepartment(string departmentId)
        {
            var data = from s in _context.Departments
                       where EF.Functions.Like(s.Id.ToString(), departmentId) && s.Is_Active == true
                       orderby s.ts descending
                       select s;
            List<Department> departData = data.ToList<Department>();
            return departData;
        }
        public Department updateDept(Department departData)
        {
            var updateDepartment = new Department()
            {
                Id = departData.Id,
                Name = departData.Name,
                Is_Active = true,
                ts = DateTime.Now

            };
            _context.Departments.Update(updateDepartment);
            _context.SaveChanges();
            return departData;
        }
        public Boolean delDepartment(Department deptData)
        {
            var updateDepartment = new Department()
            {
                Id = deptData.Id,
                Name = deptData.Name,
                Is_Active = false,
                ts = DateTime.Now
            };
            try
            {
                _context.Departments.Update(updateDepartment);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Record does not exist in the database");
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
    }
}
