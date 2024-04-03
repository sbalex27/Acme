using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acme.Data;
using Acme.Models;
using Acme.Http.Requests;
using LinkGenerator = Acme.Services.LinkGenerator;
using AutoMapper;
using Acme.Profiles;

namespace Acme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly AcmeContext _context;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public FormsController(AcmeContext context, LinkGenerator linkGenerator, IMapper mapper)
        {
            _context = context;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        // GET: api/Forms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormDTO>>> GetForm()
        {
            var forms = await _context.Form
                .AsNoTracking()
                .Include(form => form.Fields)
                .ToListAsync();

            var formsDTO = _mapper.Map<List<Form>, List<FormDTO>>(forms);

            return Ok(formsDTO);
        }

        // GET: api/Forms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormDTO>> GetForm(int id)
        {
            var form = await _context.Form
                .Include(form => form.Fields)
                .ThenInclude(field => field.Values)
                .FirstAsync(e => e.Id == id);

            if (form == null)
            {
                return NotFound();
            }

            return _mapper.Map<FormDTO>(form);
        }

        // PUT: api/Forms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForm(int id, FormDTO formDto)
        {
            if (id != formDto.Id)
            {
                return BadRequest();
            }

            if (!FormExists(id))
            {
                return NotFound();
            }

            var form = _mapper.Map<Form>(formDto);
            _context.Entry(form).State = EntityState.Modified;

            try
            {
                foreach (var field in form.Fields)
                {
                    // If the field is new, add it to the context else update it
                    if (field.Id == default)
                    {
                        field.Form = form;
                        _context.Field.Add(field);
                    }
                    else
                    {
                        _context.Entry(field).State = EntityState.Modified;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(id))
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

        // POST: api/Forms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Form>> PostForm(FormDTO formDTO)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var link = _linkGenerator.GenerateFormName(formDTO.Name);

            if (await _context.Form.AnyAsync(form => form.Link == link))
            {
                ModelState.AddModelError("Name", "Ya existe un formulario con este Nombre/Link");
                return BadRequest(ModelState);
            }

            var form = _mapper.Map<Form>(formDTO);
            form.Link = link;

            _context.Form.Add(form);
            await _context.SaveChangesAsync();

            var formDTOToReturn = _mapper.Map<FormDTO>(form);

            return CreatedAtAction("GetForm", new { id = form.Id }, formDTOToReturn);
        }

        // DELETE: api/Forms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var form = await _context.Form.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            _context.Form.Remove(form);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FormExists(int id)
        {
            return _context.Form.Any(e => e.Id == id);
        }
    }
}
