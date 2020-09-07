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
        private ThankCardsService _thankcardservice;

        public ThankCardsController(ApplicationContext context,ThankCardsService thankcardservice)
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
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in ThankCards Table
            if (thankcards.Count() > 0)
            {
                // Updating Row data with Client Update Data

                List<object> thankcardlist = _thankcardservice.updateThankCardView(id,status);

                //Return Updated Result to Client with JSON format
                returndata.Add(thankcardlist);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            else
            {
                // There is no data to update
                retdata.statuscode = "304";
                retdata.status = "No Data To Update";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
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
            List<object> returnstatus = new List<object>();
            ReturnData retdata = new ReturnData();

            //Checking Data Have or Not in ThankCards Table
            if (thankcards.Count() > 0)
            {
                // Updating Row data with Client Update Data

                List<object> thankcardlist = _thankcardservice.updateThankCardReply(id, status, reply_text);

                //Return Updated Result to Client with JSON format
                returndata.Add(thankcardlist);
                retdata.statuscode = "200";
                retdata.status = "Success";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            else
            {
                // There is no data to update
                retdata.statuscode = "304";
                retdata.status = "No Data To Update";
                returnstatus.Add(retdata);
                result["status"] = returnstatus;
                result["subdepartment"] = returndata;
            }
            return JsonConvert.SerializeObject(result);
        }
        private bool ThankCardExists(long? id)
        {
            return _context.ThankCards.Any(e => e.Id == id);
        }
    }
}
