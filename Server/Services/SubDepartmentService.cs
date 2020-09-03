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
       
        List<SubDepartment> getSubDepartment(string subdptid);
        SubDepartment CreateSubDepartment(SubDepartment subdptdata);
        SubDepartment UpdateSubDepartment(SubDepartment subdptdata);
        Boolean DeleteSubDepartment(SubDepartment subdata);

    }
    public class SubDepartmentService : ISubDepartmentService
    {
        private ApplicationContext _context;

        public SubDepartmentService(ApplicationContext context)
        {
            _context = context;
        }
        public List<SubDepartment> getSubDepartment(string subdptid)
        {
            List<SubDepartment> subdptlist = new List<SubDepartment>();
            var data = from s in _context.SubDepartments
                       where EF.Functions.Like(s.Id.ToString(), subdptid) && s.Is_Active == 1
                       select s;
            subdptlist = data.ToList<SubDepartment>();
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
        public Boolean DeleteSubDepartment(SubDepartment subdata)
        {
            Boolean retresult = true;
            var updatesub = new SubDepartment()
            {
                Id = subdata.Id,
                Name = subdata.Name,
                Dept_Id = subdata.Dept_Id,
                Is_Active = subdata.Is_Active,
                ts = subdata.ts
            };
            try
            {
                _context.SubDepartments.Update(updatesub);
                _context.SaveChanges();
                retresult = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("SubDepartment does not exist in the database");
                retresult = false;
            }
            catch (Exception ex)
            {
                throw;
                retresult = false;
            }
            return retresult;
        }
    }
}
