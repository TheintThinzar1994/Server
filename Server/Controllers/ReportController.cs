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
    public class ReportController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IReportService _reportservice; // fix the service by ttzh

        public ReportController(ApplicationContext context,IReportService reportservice)
        {
            _context = context;
            _reportservice = reportservice;
        }

        // GET: api/Report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThankCard>>> GetThankCards()
        {
            return await _context.ThankCards.ToListAsync();
        }

        // GET: api/Report/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThankCard>> GetThankCard(long? id)
        {
            var thankCard = await _context.ThankCards.FindAsync(id);

            if (thankCard == null)
            {
                return NotFound();
            }

            return thankCard;
        }

        // PUT: api/Report/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThankCard(long? id, ThankCard thankCard)
        {
            if (id != thankCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(thankCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThankCardExists(id))
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

        // POST: api/Report
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ThankCard>> PostThankCard(ThankCard thankCard)
        {
            _context.ThankCards.Add(thankCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThankCard", new { id = thankCard.Id }, thankCard);
        }

        // DELETE: api/Report/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ThankCard>> DeleteThankCard(long? id)
        {
            var thankCard = await _context.ThankCards.FindAsync(id);
            if (thankCard == null)
            {
                return NotFound();
            }

            _context.ThankCards.Remove(thankCard);
            await _context.SaveChangesAsync();

            return thankCard;
        }
       
        //Get Received ThankCard Total By Department -SSM 08/09/2020
        [HttpGet]
        [Route("GetThankCardTotalByDept")]
        public string getThankCardTotalByDepartment(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getThankCardTotalByDepartment(dept_id,sub_dept_id, from_date, to_date, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }
        
        //Get Received ThankCard Total By Employee -- SSM 08/09/2020
        [HttpGet]
        [Route("GetThankCardTotalByEmployee")]
        public string getReceivedThankCardTotalByEmployee(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string to_emp_id = (string)arr["to_emp_id"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getThankCardTotalByEmployee(dept_id, sub_dept_id, from_date, to_date,to_emp_id, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }

        //Get Received ThankCard Detail Data By Employee -- SSM 08/09/2020
        [HttpGet]
        [Route("GetThankCardTotalByEmployeeView")]
        public string getReceivedThankCardTotalByEmployeeView(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string to_emp_id = (string)arr["to_emp_id"];
            string from_emp_id = (string)arr["from_emp_id"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getThankCardTotalByEmployeeView(dept_id, sub_dept_id, from_date, to_date, from_emp_id , to_emp_id, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }

        //Get Sent ThankCard Total By Employee -- SSM 08/09/2020
        [HttpGet]
        [Route("GetSentThankCardTotalByEmployee")]
        public string getSentThankCardTotalByEmployee(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string from_emp_id = (string)arr["from_emp_id"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getSentThankCardTotalByEmployee(dept_id, sub_dept_id, from_date, to_date, from_emp_id, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }

        //Get Received ThankCard Detail Data By Employee -- SSM 08/09/2020
        [HttpGet]
        [Route("GetSentThankCardTotalByEmployeeView")]
        public string getSentThankCardTotalByEmployeeView(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string from_emp_id = (string)arr["from_emp_id"];
            string to_emp_id = (string)arr["to_emp_id"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getSentThankCardTotalByEmployeeView(dept_id, sub_dept_id, from_date, to_date, from_emp_id,to_emp_id, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }

        //Get Department Relationship ThankCard Total  -SSM 08/09/2020
        [HttpGet]
        [Route("GetThankCardTotalByDeptRelation")]
        public string getThankCardDepartmentRelationship(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string from_dept_id = (string)arr["from_dept_id"];
            string to_dept_id = (string)arr["to_dept_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];
            string order = (string)arr["order"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _reportservice.getThankCardTotalByDeparmentRelation(from_dept_id, to_dept_id, from_date, to_date, order);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }
        private bool ThankCardExists(long? id)
        {
            return _context.ThankCards.Any(e => e.Id == id);
        }
    }
}
