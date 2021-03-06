﻿using Microsoft.EntityFrameworkCore;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IThankCardsService
    {
        List<ThankCard> updateThankCardView(int id, string status);
        List<object> updateThankCardReply(int id, string status, string reply);

        List<object> getEmployee(string dept_id, string sub_dept_id, string to_emp_id,string from_emp_id);
        ThankCard CreateThankCards(ThankCard thakcard);
        List<object> getGiveThankView(int id);
        List<object> getGiveCardList(string from_emp_id,string to_emp_id, DateTime from_date, DateTime to_date, string to_d_id, string to_s_id);
        List<object> getFromGiveCardListFromEmployee(string from_emp_id,string to_emp_id, DateTime from_date, DateTime to_date,string f_dept_id,string f_s_dept_id);

    }
    public class ThankCardsService : IThankCardsService
    {
        private ApplicationContext _context;

        public ThankCardsService(ApplicationContext context)
        {
            _context = context;
        }

        public ThankCard CreateThankCards(ThankCard thakcard)
        {
            _context.ThankCards.Add(thakcard);
            _context.SaveChanges();
            return thakcard;
        }
        public List<ThankCard> updateThankCardView(int id,string status)
        {
            List<ThankCard> thankcard = new List<ThankCard>();
            var data1 = from s in _context.ThankCards
                        where s.Id == id && s.isActive==true
                        select s;
            thankcard = data1.ToList<ThankCard>();
            if (thankcard.Count() > 0)
            {
                ThankCard objthank = new ThankCard();
                objthank = thankcard[0];
                objthank.Status = status;
                objthank.ts = DateTime.Now;
                objthank.isActive = true;
                _context.ThankCards.Update(objthank);
                _context.SaveChanges();
            }
            
            data1 = from s in _context.ThankCards
                   // join e in _context.Employees on s.From_Employee_Id equals e.Id
                    where s.Id == id && s.isActive == true
                    select s;

            List<ThankCard> ret_thankcard = data1.ToList<ThankCard>();
            return ret_thankcard;
        }
        public List<object> updateThankCardReply(int id, string status,string reply)
        {
            List<ThankCard> thankcard = new List<ThankCard>();
            var data1 = from s in _context.ThankCards
                        where s.Id == id && s.isActive == true
                        select s;
            thankcard = data1.ToList<ThankCard>();
            if (thankcard.Count() > 0)
            {
                ThankCard objthank = new ThankCard();
                objthank = thankcard[0];
                objthank.Status = status;
                objthank.ReplyDate = DateTime.Now;
                objthank.ReplyText = reply;
                objthank.ts = DateTime.Now;
                objthank.isActive = true;
                _context.ThankCards.Update(objthank);
                _context.SaveChanges();
            }
            data1 = from s in _context.ThankCards
                    where s.Id == id && s.isActive == true
                    select s;
            List<object> ret_thankcard = data1.ToList<object>();
            return ret_thankcard;
        }
        public List<object> getEmployee(string dept_id, string sub_dept_id, string to_emp_id,string from_emp_id)
        {
            List<object> emplist = new List<object>();
            var data1 = from e in _context.Employees.Where(e=>e.Id.ToString()!=from_emp_id)
                        join s in _context.SubDepartments on e.Sub_Dept_Id equals s.Id
                        join d in _context.Departments on e.Dept_Id equals d.Id
                        where EF.Functions.Like(s.Id.ToString(), sub_dept_id) && e.isActive == true
                        && s.Is_Active == 1 && d.Is_Active == true
                        && EF.Functions.Like(d.Id.ToString(), dept_id) && EF.Functions.Like(e.Id.ToString(), to_emp_id)
                        select new
                        {
                            Emp_Id = e.Id,
                            Emp_Name = e.User_Name,
                            PhotoName = e.PhotoName,
                            sub_dept_id = s.Id,
                            Sub_Dept_Name = s.Name,
                            dept_id = d.Id,
                            Dept_Name = d.Name

                        };
            emplist = data1.ToList<object>();
            return emplist;
        }
        //Update by SSM to check Department and SubDepartment of each Employee on 15/09/2020
        public List<object> getGiveThankView(int id)
        {
            List<object> retdata = new List<object>();
            var data1 = from s in _context.ThankCards
                        join femp in _context.Employees on s.From_Employee_Id equals femp.Id
                        join temp in _context.Employees on s.To_Employee_Id equals temp.Id
                        join fd in _context.Departments on femp.Dept_Id equals fd.Id
                        join fs in _context.SubDepartments on femp.Sub_Dept_Id equals fs.Id
                        join td in _context.Departments on temp.Dept_Id equals td.Id
                        join ts in _context.SubDepartments on temp.Sub_Dept_Id equals ts.Id
                        where s.Id == id && s.isActive==true && femp.isActive==true && temp.isActive==true
                        && fd.Is_Active==true && fs.Is_Active==1 && td.Is_Active==true && ts.Is_Active==1
                        select new
                        {
                            s.Id,s.From_Employee_Id,s.To_Employee_Id,From_Employee_Name=femp.User_Name,
                            From_Employee_Photo = femp.PhotoName, To_Employee_Photo = temp.PhotoName,
                            To_Employee_Name = temp.User_Name,s.Title,s.SendDate,s.SendText,s.ReplyDate,s.ReplyText,
                            s.Status
                        };
            retdata = data1.ToList<object>();
            return retdata;
        }

        //update by ssm adding ToDepartment,ToSubDepartment Filters 14/09/2020
        public List<object> getGiveCardList(string from_emp_id,string to_emp_id, DateTime from_date, DateTime to_date,string to_d_id,string to_s_id)
        {
            List<object> retdata = new List<object>();
            DateTime f_date = Convert.ToDateTime(from_date.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(to_date.ToString("yyyy-MM-dd 23:59:59"));
            //DateTime.Now.ToString("yyyy-MM-dd 00:00:00")
            var data1 = from tc in _context.ThankCards
                        join femp in _context.Employees on tc.From_Employee_Id equals femp.Id
                        join temp in _context.Employees on tc.To_Employee_Id equals temp.Id
                        join d in _context.Departments on temp.Dept_Id equals d.Id
                        join sd in _context.SubDepartments on temp.Sub_Dept_Id equals sd.Id
                        where EF.Functions.Like(tc.To_Employee_Id.ToString(), to_emp_id) && tc.isActive == true &&
                        femp.isActive==true && temp.isActive==true && d.Is_Active==true && sd.Is_Active==1 &&
                        EF.Functions.Like(d.Id.ToString(),to_d_id) && EF.Functions.Like(sd.Id.ToString(),to_s_id) &&
                        (tc.SendDate>=f_date && tc.SendDate<=t_date) && tc.From_Employee_Id.ToString()== from_emp_id && femp.isActive == true
                        orderby tc.SendDate descending
                        select new
                        {
                            Emp_Name = temp.User_Name,
                            Thankid = tc.Id,
                            Dept_Name = d.Name,
                            Sub_Dept_Name = sd.Name,
                            Date = tc.SendDate,
                            Status = tc.Status,
                            Thank_Id =tc.Id
                        };
            retdata = data1.ToList<object>();
            return retdata;

        }
        public List<object> getFromGiveCardListFromEmployee(string from_emp_id,string to_emp_id, DateTime from_date, DateTime to_date,string f_dept_id,string f_s_dept_id)
        {
            DateTime f_date = Convert.ToDateTime(from_date.ToString("yyyy-MM-dd 00:00:00"));
            DateTime t_date = Convert.ToDateTime(to_date.ToString("yyyy-MM-dd 23:59:59"));
            List<object> retdata = new List<object>();
            var data = from tc in _context.ThankCards
            join fe in _context.Employees on tc.From_Employee_Id equals fe.Id
            join te in _context.Employees on tc.To_Employee_Id equals te.Id
            join d in _context.Departments on fe.Dept_Id equals d.Id
            join sd in _context.SubDepartments on fe.Sub_Dept_Id equals sd.Id
            where EF.Functions.Like(tc.From_Employee_Id.ToString(), from_emp_id) && tc.isActive == true 
            && fe.isActive==true && te.isActive==true && d.Is_Active==true && sd.Is_Active==1
            && tc.To_Employee_Id.ToString()==to_emp_id 
            && EF.Functions.Like(d.Id.ToString(),f_dept_id) && 
            EF.Functions.Like(sd.Id.ToString(),f_s_dept_id) &&
            (tc.SendDate >= f_date  && tc.SendDate <= t_date)
            orderby tc.SendDate descending
                       select new
                       {
                           Emp_Name = fe.User_Name,
                           Dept_Name = d.Name,
                           Sub_Dept_Name = sd.Name,
                           Date = tc.SendDate,
                           Status = tc.Status,
                           Id =tc.Id
                       };
            retdata = data.ToList<object>();
            return retdata;

        }


    }
}
