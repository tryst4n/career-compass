using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerCompass.Data;
using CareerCompass.Models;

namespace CareerCompass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVsController : ControllerBase
    {
        private readonly CareerCompassContext _context;

        public CVsController(CareerCompassContext context)
        {
            _context = context;
        }

        // GET: api/CVs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CV>>> GetCV()
        {
            return await _context.CV.ToListAsync();
        }

        // GET: api/CVs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CV>> GetCV(int id)
        {
            var cV = await _context.CV.FindAsync(id);

            if (cV == null)
            {
                return NotFound();
            }

            return cV;
        }

        // PUT: api/CVs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCV(int id, CV cV)
        {
            if (id != cV.Id)
            {
                return BadRequest();
            }

            _context.Entry(cV).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CVExists(id))
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

        // POST: api/CVs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CV>> PostCV(CV cV)
        {
            _context.CV.Add(cV);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCV", new { id = cV.Id }, cV);
        }

        // DELETE: api/CVs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCV(int id)
        {
            var cV = await _context.CV.FindAsync(id);
            if (cV == null)
            {
                return NotFound();
            }

            _context.CV.Remove(cV);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CVExists(int id)
        {
            return _context.CV.Any(e => e.Id == id);
        }
    }
}
