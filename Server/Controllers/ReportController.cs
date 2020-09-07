using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Model;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ReportController(ApplicationContext context)
        {
            _context = context;
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

        private bool ThankCardExists(long? id)
        {
            return _context.ThankCards.Any(e => e.Id == id);
        }
    }
}
