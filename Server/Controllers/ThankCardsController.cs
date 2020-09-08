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
    public class ThankCardsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private IThankCardsService _thankcardservice; // fix the service by ttzh

        public ThankCardsController(ApplicationContext context, IThankCardsService thankcardservice)
        {
            _context = context;
            _thankcardservice = thankcardservice;
        }

        // GET: api/ThankCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThankCard>>> GetThankCards()
        {
            return await _context.ThankCards.ToListAsync();
        }

        // GET: api/ThankCards/5
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

        // PUT: api/ThankCards/5
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

        // POST: api/ThankCards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ThankCard>> PostThankCard(ThankCard thankCard)
        {
            _context.ThankCards.Add(thankCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThankCard", new { id = thankCard.Id }, thankCard);
        }

        // DELETE: api/ThankCards/5
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
        [HttpPut]
        [Route("UpdateView")]
        public string UpdateThankCardView(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
            int id = (int)arr["Id"];
            string status = (string)arr["status"];

            //Checking Data Have or Not in ThanksCards Table
            var thankcards = _context.ThankCards.Where(e => e.Id==id && e.isActive==true);

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> empreturndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in ThankCards Table
            if (thankcards.Count() > 0)
            {
                // Updating Row data with Client Update Data

                List<ThankCard> thankcardlist = _thankcardservice.updateThankCardView(id,status);

                // list employee data
                if (thankcardlist.Count() > 0)
                {
                    ThankCard objcard = new ThankCard();
                    objcard = (ThankCard)thankcardlist[0];
                    List<object> fromempdata = _thankcardservice.getEmployee("%", "%", objcard.From_Employee_Id.ToString());
                    empreturndata.Add(fromempdata);
                    result["fromEmpData"] = empreturndata;
                }


                //Return Updated Result to Client with JSON format
                returndata.Add(thankcardlist);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["thankcards"] = returndata;
            }
            else
            {
                // There is no data to update
                retdata.statuscode = "304";
                retdata.status = "No Data To Update";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["thankcards"] = returndata;
            }
            return JsonConvert.SerializeObject(result);
        }

        [HttpPut]
        [Route("UpdateReply")]
        public string UpdateThankCardReply(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
            int id = (int)arr["Id"];
            string status = (string)arr["status"];
            string reply_text = (string)arr["reply"];

            //Checking Data Have or Not in ThanksCards Table
            var thankcards = _context.ThankCards.Where(e => e.Id == id && e.isActive == true);

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> emprdata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in ThankCards Table
            if (thankcards.Count() > 0)
            {
                // Updating Row data with Client Update Data

                List<object> thankcardlist = _thankcardservice.updateThankCardReply(id, status, reply_text);
                // Get employee data from Employee
                if (thankcardlist.Count() > 0)
                {
                    ThankCard objcard1 = new ThankCard();
                    objcard1 = (ThankCard)thankcardlist[0];
                    List<object> fromempdata1 = _thankcardservice.getEmployee("%", "%", objcard1.From_Employee_Id.ToString());
                    emprdata.Add(fromempdata1);
                    result["fromEmpData"] = emprdata;
                }

                //Return Updated Result to Client with JSON format
                returndata.Add(thankcardlist);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["thankcards"] = returndata;
            }
            else
            {
                // There is no data to update
                retdata.statuscode = "304";
                retdata.status = "No Data To Update";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["thankcards"] = returndata;
            }
            return JsonConvert.SerializeObject(result);
        }
        [HttpGet]
        [Route("GetEmployee")]
        public string getThankCardEmployee(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
            string emp_id = (string)arr["emp_id"];
            string dept_id = (string)arr["dept_id"];
            string sub_dept_id = (string)arr["sub_dept_id"];           

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> emplist = _thankcardservice.getEmployee(dept_id,sub_dept_id,emp_id);

            //Return Updated Result to Client with JSON format
            returndata.Add(emplist);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["emplist"] = returndata;
            
            return JsonConvert.SerializeObject(result);
        }
        [HttpPost]
        [Route("ThankCard")]
        public string InsertThankCard(string paramList)
        {
            var arr = JObject.Parse(paramList);
            int from_emp_id = (int)arr["from_emp_id"];
            int to_emp_id = (int)arr["to_emp_id"];
            string title = (string)arr["title"];
            string send_text = (string)arr["send_text"];
            string status = (string)arr["status"];
            

            //Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();
           
            //Accepting data comming from Client(PHP)
            ThankCard thankCard = new ThankCard();
            thankCard.From_Employee_Id = from_emp_id;
            thankCard.To_Employee_Id = to_emp_id;
            thankCard.Title = title;
            thankCard.SendText = send_text;
            thankCard.SendDate = DateTime.Now;
            thankCard.ReplyDate = DateTime.Now;
            thankCard.ReplyText = "";
            thankCard.Status = "Delivered";
            thankCard.ts = DateTime.Now;
            thankCard.isActive = true;            
            //Inserting data into tables
            ThankCard thankcardresult = _thankcardservice.CreateThankCards(thankCard);
            returndata.Add(thankcardresult);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

        return JsonConvert.SerializeObject(result);

        }
        [HttpGet]
        [Route("GetGiveCard")]
        public string getGiveCardToView(string paramList)
        {
            //Accepting data from 
            var arr = JObject.Parse(paramList);
            int Id = (int)arr["id"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _thankcardservice.getGiveThankView(Id);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }
        [HttpGet]
        [Route("GetGiveCardList")]
        public string getGiveCardListView(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string to_emp_id = (string)arr["to_emp_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _thankcardservice.getGiveCardList(to_emp_id, from_date, to_date);

            //Return Updated Result to Client with JSON format
            returndata.Add(thankcard);
            retdata.statuscode = "200";
            retdata.status = "Success";
            returnstatus.Add(retdata);
            result["status"] = returnstatus;
            result["thankcard"] = returndata;

            return JsonConvert.SerializeObject(result);
        }
        
        [HttpGet]
        [Route("ThankCardsFemp")] // Home Page List By TTZH
        public string   getGiveCardFromEmployeeList(string paramList)
        {
            //Accepting data from             
            var arr = JObject.Parse(paramList);
            string from_emp_id = (string)arr["from_emp_id"];
            DateTime from_date = (DateTime)arr["from_date"];
            DateTime to_date = (DateTime)arr["to_date"];

            ////Creating Objects for Json Returns
            IDictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<object> returndata = new List<object>();
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Getting Table Results from the Database

            List<object> thankcard = _thankcardservice.getFromGiveCardListFromEmployee(from_emp_id, from_date, to_date);

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
