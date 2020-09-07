using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IReportService
    {
        List<object> getThankCardTotalByDepartment(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate);
    }

    public class ReportService : IReportService
    {
        private ApplicationContext _context;

        public ReportService(ApplicationContext context)
        {
            _context = context;
        }
        public List<object> getThankCardTotalByDepartment(string dept_id, string sub_dept_id, DateTime fdate,DateTime tdate)
        {            
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();
            var data1 = from t in _context.ThankCards
                        join e in _context.Employees on t.To_Employee_Id equals e.Id
                        join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                        join d in _context.Departments on e.Dept_Id equals d.Id
                        where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                        && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                        select new { Dept_Name = d.Name, Sub_Dept_Name = s.Name } into result
                        group result by new { result.Dept_Name, result.Sub_Dept_Name } into g
                        select new
                        {
                            Dept_Name = g.Key.Dept_Name,
                            Sub_Dep_Name = g.Key.Sub_Dept_Name,
                            CountResult = g.Count()
                        } into resultcount
                        select new
                        {
                            resultcount.Dept_Name,resultcount.Sub_Dep_Name,resultcount.CountResult,f_date,t_date
                        }
                        ;

            emplist = data1.ToList<object>();
            return emplist;
        }
    }
}
