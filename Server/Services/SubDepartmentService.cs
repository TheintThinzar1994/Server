using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ISubDepartmentService
    {
       
        List<object> getSubDepartment(string subdptid);
        SubDepartment CreateSubDepartment(SubDepartment subdptdata);
        SubDepartment UpdateSubDepartment(SubDepartment subdptdata);
        Boolean DeleteSubDepartment(SubDepartment subdeptdata);

    }
    public class SubDepartmentService : ISubDepartmentService
    {
        private ApplicationContext _context;

        public SubDepartmentService(ApplicationContext context)
        {
            _context = context;
        }
        public List<object> getSubDepartment(string subdptid)
        {
            List<object> subdptlist = new List<object>();            
            var data1 = from s in _context.SubDepartments
                        join d in _context.Departments on s.Dept_Id equals d.Id 
                       where EF.Functions.Like(s.Id.ToString(), subdptid) && s.Is_Active == 1
                       && d.Is_Active==true
                       orderby s.ts descending
                       select new { Sub_Dept_Id=s.Id,Sub_Dept_Name=s.Name,Dept_Name=d.Name,Dept_Id=d.Id};
            subdptlist = data1.ToList<object>();
            return subdptlist;
        }
        public SubDepartment CreateSubDepartment(SubDepartment subdptdata)
        {
            _context.SubDepartments.Add(subdptdata);
            _context.SaveChanges();
            return subdptdata;
        }

        public SubDepartment UpdateSubDepartment(SubDepartment subdptdata)
        {
            var updatesubdpt = new SubDepartment()
            {
                Id = subdptdata.Id,
                Name = subdptdata.Name,
                Dept_Id=subdptdata.Dept_Id,
                Is_Active = subdptdata.Is_Active,
                ts = subdptdata.ts


            };
            _context.SubDepartments.Update(updatesubdpt);
            _context.SaveChanges();
            return updatesubdpt;
        }
        public Boolean DeleteSubDepartment(SubDepartment subdeptdata)
        {
            Boolean retresult = true;
            //var updatesub = new SubDepartment()
            //{
            //    Id = subdeptdata.Id,
            //    Name= subdeptdata.Name,
            //    Dept_Id= subdeptdata.Dept_Id,
            //    Is_Active = subdeptdata.Is_Active,
            //    ts = subdeptdata.ts
            //};
            _context.SubDepartments.Update(subdeptdata);
            _context.SaveChanges();
            retresult = true;
            //try
            //{
                
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    throw new Exception("SubDepartment does not exist in the database");
            //    retresult = false;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return retresult;
        }
    }
}
