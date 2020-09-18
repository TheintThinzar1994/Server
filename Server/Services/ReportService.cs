using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        List<object> getThankCardTotalByDepartment(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate,string order);
        List<object> getThankCardTotalByEmployee(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate,string to_emp_id, string order);
        List<object> getThankCardTotalByEmployeeView(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate, string from_emp_id,string to_emp_id, string order);
        List<object> getSentThankCardTotalByEmployee(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate, string from_emp_id, string order);
        List<object> getSentThankCardTotalByEmployeeView(string dept_id, string sub_dept_id, DateTime fdate, DateTime tdate, string from_emp_id,string to_emp_id, string order);
        List<object> getThankCardTotalByDeparmentRelation(string from_dept_id, string to_dept_id, DateTime fdate, DateTime tdate, string order);
    }

    public class ReportService : IReportService
    {
        private ApplicationContext _context;

        public ReportService(ApplicationContext context)
        {
            _context = context;
        }
        //Get Received ThankCard Total By Department -SSM 08/09/2020
        public List<object> getThankCardTotalByDepartment(string dept_id, string sub_dept_id, DateTime fdate,DateTime tdate,string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();
            
            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true 
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            select new { Dept_Name = d.Name, Sub_Dept_Name = s.Name } into result
                            group result by new { result.Dept_Name, result.Sub_Dept_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Sub_Dep_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            select new { Dept_Name = d.Name, Sub_Dept_Name = s.Name } into result
                            group result by new { result.Dept_Name, result.Sub_Dept_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult ascending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Sub_Dep_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                        ;
                emplist = data1.ToList<object>();
            }

            
            return emplist;
        }
        //Get Received ThankCard Total By Employee -- SSM 08/09/2020
        public List<object> getThankCardTotalByEmployee(
            string dept_id,
            string sub_dept_id,
            DateTime fdate, 
            DateTime tdate,
            string to_emp_id,
            string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();

            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.To_Employee_Id.ToString(), to_emp_id)
                            select new { Dept_Name = d.Name, Dept_Id = d.Id, Sub_Dept_Id = s.Id, From_Emp_Id = t.From_Employee_Id, To_Emp_Id = t.To_Employee_Id , Sub_Dept_Name = s.Name,Emp_Name = e.User_Name } into result
                            group result by new { result.Dept_Name,result.To_Emp_Id, result.Dept_Id, result.Sub_Dept_Id, result.Sub_Dept_Name,result.Emp_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Dep_Id = g.Key.Dept_Id,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                Sub_Dept_Id = g.Key.Sub_Dept_Id,
                                Emp_Name = g.Key.Emp_Name,
                                //From_Emp_Id = g.Key.From_Emp_Id,
                                To_Emp_Id = g.Key.To_Emp_Id,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Dep_Id,
                                resultcount.Sub_Dept_Id,
                                resultcount.Sub_Dep_Name,
                                resultcount.CountResult,
                                //resultcount.From_Emp_Id,
                                resultcount.To_Emp_Id,
                                resultcount.Emp_Name,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true 
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.To_Employee_Id.ToString(), to_emp_id)
                            select new { Dept_Name = d.Name,Dept_Id = d.Id,Sub_Dept_Id = s.Id, Sub_Dept_Name = s.Name, From_Emp_Id = t.From_Employee_Id,To_Emp_Id = t.To_Employee_Id, Emp_Name = e.User_Name } into result
                            group result by new { result.Dept_Name, result.Dept_Id, result.Sub_Dept_Id, 
                                //result.From_Emp_Id,
                                result.To_Emp_Id, result.Sub_Dept_Name, result.Emp_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Dep_Id = g.Key.Dept_Id,
                                Sub_Dept_Id = g.Key.Sub_Dept_Id,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                //From_Emp_Id = g.Key.From_Emp_Id,
                                To_Emp_Id = g.Key.To_Emp_Id,
                                Emp_Name = g.Key.Emp_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult ascending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Dep_Id,
                                resultcount.Sub_Dept_Id,
                                resultcount.Sub_Dep_Name,
                                resultcount.CountResult,
                                //resultcount.From_Emp_Id,
                                resultcount.To_Emp_Id,
                                resultcount.Emp_Name,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }


            return emplist;
        }
        //Get Received ThankCard Detail Data By Employee -- SSM 08/09/2020
        public List<object> getThankCardTotalByEmployeeView(
            string dept_id,
            string sub_dept_id,
            DateTime fdate,
            DateTime tdate,
            string from_emp_id,
            string to_emp_id,
            string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();

            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            join fs in _context.SubDepartments on fe.Sub_Dept_Id equals fs.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && 
                            e.isActive == true && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)&&
                            EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)
                            && EF.Functions.Like(t.To_Employee_Id.ToString(), to_emp_id)
                            select new { Dept_Name = d.Name, Sub_Dept_Name = s.Name,To_Emp_Name = e.User_Name,From_Emp_Name=fe.User_Name,
                                        Send_Date = t.SendDate,From_Dept_Name = fd.Name,From_Sub_Dept_Name = fs.Name
                                       } into result
                            group result by new { result.Dept_Name, result.Sub_Dept_Name, result.To_Emp_Name,result.From_Emp_Name,
                                                result.From_Dept_Name,result.From_Sub_Dept_Name,result.Send_Date
                                                 } into g
                            select new
                            {
                                To_Dep_Name = g.Key.Dept_Name,
                                To_Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                To_Emp = g.Key.To_Emp_Name,
                                From_Emp = g.Key.From_Emp_Name,
                                From_Dep_Name = g.Key.From_Dept_Name,
                                From_Sub_Dep_Name = g.Key.From_Sub_Dept_Name,
                                Send_Date = g.Key.Send_Date,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.From_Emp
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.To_Dep_Name,
                                resultcount.To_Sub_Dep_Name,
                                resultcount.To_Emp,
                                resultcount.From_Emp,
                                resultcount.From_Dep_Name,
                                resultcount.From_Sub_Dep_Name,
                                resultcount.CountResult,
                                resultcount.Send_Date,
                                f_date,
                                t_date,

                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            join fs in _context.SubDepartments on fe.Sub_Dept_Id equals fs.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true &&
                            t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)&&
                            EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)
                            && EF.Functions.Like(t.To_Employee_Id.ToString(), to_emp_id)
                            select new
                            {
                                Dept_Name = d.Name,
                                Sub_Dept_Name = s.Name,
                                To_Emp_Name = e.User_Name,
                                From_Emp_Name = fe.User_Name,
                                From_Dept_Name = fd.Name,
                                From_Sub_Dept_Name = fs.Name,
                                Send_Date = t.SendDate
                            } into result
                            group result by new
                            {
                                result.Dept_Name,
                                result.Sub_Dept_Name,
                                result.To_Emp_Name,
                                result.Send_Date,
                                result.From_Emp_Name,
                                result.From_Dept_Name,
                                result.From_Sub_Dept_Name,
                            } into g
                            select new
                            {
                                To_Dep_Name = g.Key.Dept_Name,
                                To_Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                To_Emp = g.Key.To_Emp_Name,
                                Send_Date = g.Key.Send_Date,
                                From_Emp = g.Key.From_Emp_Name,
                                From_Dep_Name = g.Key.From_Dept_Name,
                                From_Sub_Dep_Name = g.Key.From_Sub_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.From_Emp
                            orderby resultcount.CountResult ascending
                            select new
                            {
                                resultcount.To_Dep_Name,
                                resultcount.To_Sub_Dep_Name,
                                resultcount.Send_Date,
                                resultcount.To_Emp,
                                resultcount.From_Emp,
                                resultcount.From_Dep_Name,
                                resultcount.From_Sub_Dep_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }


            return emplist;
        }

        //Get Sent ThankCard Total By Employee -- SSM 08/09/2020
        public List<object> getSentThankCardTotalByEmployee(
           string dept_id,
           string sub_dept_id,
           DateTime fdate,
           DateTime tdate,
           string from_emp_id,
           string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();

            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.From_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)
                            select new { Dept_Name = d.Name,Dept_Id = d.Id,Sub_Dept_Id = s.Id, Sub_Dept_Name = s.Name,From_Employee_Id =t.From_Employee_Id, To_Employee_Id = t.To_Employee_Id, Emp_Name = e.User_Name } into result
                            group result by new { result.Dept_Name,result.Dept_Id,result.Sub_Dept_Id,result.Sub_Dept_Name,result.From_Employee_Id,
                                //result.To_Employee_Id, 
                                result.Emp_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Dep_Id = g.Key.Dept_Id,
                                Sub_Dept_Id = g.Key.Sub_Dept_Id,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                From_Emp_Id = g.Key.From_Employee_Id,
                                //To_Emp_Id = g.Key.To_Employee_Id,
                                Emp_Name = g.Key.Emp_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Dep_Id,
                                resultcount.Sub_Dep_Name,
                                resultcount.Sub_Dept_Id,
                                resultcount.From_Emp_Id,
                                //resultcount.To_Emp_Id,
                                resultcount.CountResult,
                                resultcount.Emp_Name,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.From_Employee_Id equals e.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(d.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)
                            select new { Dept_Name = d.Name, Dept_Id = d.Id, Sub_Dept_Id = s.Id, Sub_Dept_Name = s.Name,From_Emp_Id = t.From_Employee_Id,To_Emp_Id = t.To_Employee_Id, Emp_Name = e.User_Name } into result
                            group result by new { result.Dept_Name, result.Sub_Dept_Name, result.Dept_Id, result.Sub_Dept_Id,result.From_Emp_Id ,
                                //result.To_Emp_Id,
                                result.Emp_Name } into g
                            select new
                            {
                                Dept_Name = g.Key.Dept_Name,
                                Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                Dep_Id = g.Key.Dept_Id,
                                Sub_Dept_Id = g.Key.Sub_Dept_Id,
                                From_Emp_Id = g.Key.From_Emp_Id,
                                //To_Emp_Id = g.Key.To_Emp_Id,
                                Emp_Name = g.Key.Emp_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult ascending
                            select new
                            {
                                resultcount.Dept_Name,
                                resultcount.Dep_Id,
                                resultcount.Sub_Dep_Name,
                                resultcount.Sub_Dept_Id,
                                resultcount.CountResult,
                                resultcount.From_Emp_Id,
                                //resultcount.To_Emp_Id,
                                resultcount.Emp_Name,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }


            return emplist;
        }

        //Get Sent ThankCard Detail Data By Employee -- SSM 08/09/2020
        public List<object> getSentThankCardTotalByEmployeeView(
            string dept_id,
            string sub_dept_id,
            DateTime fdate,
            DateTime tdate,
            string from_emp_id,
            string to_emp_id,
            string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();

            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            join fs in _context.SubDepartments on fe.Sub_Dept_Id equals fs.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            where EF.Functions.Like(fs.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(fd.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)&&
                               EF.Functions.Like(t.To_Employee_Id.ToString(),to_emp_id)
                            select new
                            {
                                Dept_Name = d.Name,
                                Sub_Dept_Name = s.Name,
                                To_Emp_Name = e.User_Name,
                                Send_Date = t.SendDate,
                                From_Emp_Name = fe.User_Name,
                                From_Dept_Name = fd.Name,
                                From_Sub_Dept_Name = fs.Name
                            } into result
                            group result by new
                            {
                                result.Dept_Name,
                                result.Sub_Dept_Name,
                                result.To_Emp_Name,
                                result.Send_Date,
                                result.From_Emp_Name,
                                result.From_Dept_Name,
                                result.From_Sub_Dept_Name,
                            } into g
                            select new
                            {
                                To_Dep_Name = g.Key.Dept_Name,
                                To_Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                To_Emp = g.Key.To_Emp_Name,
                                Send_Date =g.Key.Send_Date,
                                From_Emp = g.Key.From_Emp_Name,
                                From_Dep_Name = g.Key.From_Dept_Name,
                                From_Sub_Dep_Name = g.Key.From_Sub_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.To_Emp
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.To_Dep_Name,
                                resultcount.To_Sub_Dep_Name,
                                resultcount.To_Emp,
                                resultcount.From_Emp,
                                resultcount.Send_Date,
                                resultcount.From_Dep_Name,
                                resultcount.From_Sub_Dep_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join e in _context.Employees on t.To_Employee_Id equals e.Id
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                            join d in _context.Departments on e.Dept_Id equals d.Id
                            join fs in _context.SubDepartments on fe.Sub_Dept_Id equals fs.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            where EF.Functions.Like(fs.Id.ToString(), sub_dept_id) && e.isActive == true
                            && t.isActive==true
                            && EF.Functions.Like(fd.Id.ToString(), dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            && EF.Functions.Like(t.From_Employee_Id.ToString(), from_emp_id)&&
                               EF.Functions.Like(t.To_Employee_Id.ToString(), to_emp_id)
                            select new
                            {
                                Dept_Name = d.Name,
                                Sub_Dept_Name = s.Name,
                                To_Emp_Name = e.User_Name,
                                From_Emp_Name = fe.User_Name,
                                Send_Date = t.SendDate,
                                From_Dept_Name = fd.Name,
                                From_Sub_Dept_Name = fs.Name
                            } into result
                            group result by new
                            {
                                result.Dept_Name,
                                result.Sub_Dept_Name,
                                result.To_Emp_Name,
                                result.From_Emp_Name,
                                result.From_Dept_Name,
                                result.Send_Date,
                                result.From_Sub_Dept_Name,
                            } into g
                            select new
                            {
                                To_Dep_Name = g.Key.Dept_Name,
                                To_Sub_Dep_Name = g.Key.Sub_Dept_Name,
                                To_Emp = g.Key.To_Emp_Name,
                                From_Emp = g.Key.From_Emp_Name,
                                Send_Date = g.Key.Send_Date,
                                From_Dep_Name = g.Key.From_Dept_Name,
                                From_Sub_Dep_Name = g.Key.From_Sub_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.To_Emp
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.To_Dep_Name,
                                resultcount.To_Sub_Dep_Name,
                                resultcount.To_Emp,
                                resultcount.From_Emp,
                                resultcount.From_Dep_Name,
                                resultcount.Send_Date,
                                resultcount.From_Sub_Dep_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }


            return emplist;
        }

        //Get Received ThankCard Total By Department -SSM 08/09/2020
        public List<object> getThankCardTotalByDeparmentRelation(string from_dept_id, string to_dept_id, DateTime fdate, DateTime tdate, string order)
        {
            DateTime f_date = Convert.ToDateTime(fdate.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(tdate.ToString("yyyy-MM-dd 23:59:59"));
            List<object> emplist = new List<object>();

            if (order.ToLower() == "Desc".ToLower())
            {
                var data1 = from t in _context.ThankCards
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join te in _context.Employees on t.To_Employee_Id equals te.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            join td in _context.Departments on te.Dept_Id equals td.Id
                            where EF.Functions.Like(fe.Dept_Id.ToString(), from_dept_id) && t.isActive == true
                            && fe.isActive==true && te.isActive==true
                            && EF.Functions.Like(te.Dept_Id.ToString(), to_dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            select new { From_Dept_Name = fd.Name, To_Dept_Name = td.Name } into result
                            group result by new { result.From_Dept_Name, result.To_Dept_Name } into g
                            select new
                            {
                                F_Dept_Name = g.Key.From_Dept_Name,
                                T_Dept_Name = g.Key.To_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult descending
                            select new
                            {
                                resultcount.F_Dept_Name,
                                resultcount.T_Dept_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }

            else
            {
                var data1 = from t in _context.ThankCards
                            join fe in _context.Employees on t.From_Employee_Id equals fe.Id
                            join te in _context.Employees on t.From_Employee_Id equals te.Id
                            join fd in _context.Departments on fe.Dept_Id equals fd.Id
                            join td in _context.Departments on te.Dept_Id equals td.Id
                            where EF.Functions.Like(fd.Id.ToString(), from_dept_id) && t.isActive == true
                            && fe.isActive==true && te.isActive==true
                            && EF.Functions.Like(td.Id.ToString(), to_dept_id) && (t.SendDate >= f_date && t.SendDate <= t_date)
                            select new { From_Dept_Name = fd.Name, To_Dept_Name = td.Name } into result
                            group result by new { result.From_Dept_Name, result.To_Dept_Name } into g
                            select new
                            {
                                F_Dept_Name = g.Key.From_Dept_Name,
                                T_Dept_Name = g.Key.To_Dept_Name,
                                CountResult = g.Count()
                            } into resultcount
                            orderby resultcount.CountResult ascending
                            select new
                            {
                                resultcount.F_Dept_Name,
                                resultcount.T_Dept_Name,
                                resultcount.CountResult,
                                f_date,
                                t_date
                            }
                          ;
                emplist = data1.ToList<object>();
            }


            return emplist;
        }

    }
}
