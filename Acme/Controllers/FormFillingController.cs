using Acme.Data;
using Acme.Models;
using Acme.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormFillingController : ControllerBase
    {
        private readonly AcmeContext _context;
        private readonly IMapper _mapper;

        public FormFillingController(AcmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{formName}")]
        public async Task<ActionResult<FormDTO>> GetForm(string formName)
        {
            var form = await _context.Form
                .Include(form => form.Fields)
                .FirstOrDefaultAsync(e => e.Name == formName);

            if (form == null)
            {
                return NotFound();
            }

            var mapped = _mapper.Map<FormDTO>(form);

            return Ok(mapped);
        }

        [HttpPost("{formName}")]
        public async Task<ActionResult<FormDTO>> PostFormFilling(string formName, [FromBody] FormFillRequest request)
        {
            var form = await _context.Form
                .Include(form => form.Fields)
                .FirstOrDefaultAsync(e => e.Name == formName);

            if (form == null)
            {
                return NotFound();
            }

            foreach (var field in form.Fields)
            {
                var userValue = request.Fields.FirstOrDefault(v => v.Name == field.Name);
                var emptyValue = userValue?.Value == null;

                if (!field.IsRequired && emptyValue)
                {
                    continue;
                }

                if (field.IsRequired && emptyValue)
                {
                    // Add error to ModelState
                    ModelState.AddModelError(field.Name, $"El campo {field.Name} es obligatorio");
                    continue;
                }

                if (field.Type == FieldType.Text)
                {
                    if (userValue!.Value is not string textValue || string.IsNullOrWhiteSpace(textValue))
                    {
                        // Add error to ModelState
                        ModelState.AddModelError(field.Name, $"El campo {field.Name} debe ser de tipo texto");
                        continue;
                    }

                    _context.FieldValue.Add(new FieldValue
                    {
                        Field = field,
                        TextValue = textValue,
                        CreatedAt = DateTime.UtcNow,
                        GuestName = request.FilledBy,
                    });
                }
                else if (field.Type == FieldType.Number)
                {
                    var isNumber = int.TryParse(userValue!.Value.ToString(), out var numberValue);
                    if (!isNumber)
                    {
                        // Add error to ModelState
                        ModelState.AddModelError(field.Name, $"El campo {field.Name} debe ser de tipo número");
                        continue;
                    }

                    _context.FieldValue.Add(new FieldValue
                    {
                        Field = field,
                        NumberValue = numberValue,
                        CreatedAt = DateTime.UtcNow,
                        GuestName = request.FilledBy,
                    });
                }
                else if (field.Type == FieldType.Date)
                {
                    var isDate = DateTime.TryParse(userValue!.Value.ToString(), out var dateValue);
                    if (!isDate)
                    {
                        // Add error to ModelState
                        ModelState.AddModelError(field.Name, $"El campo {field.Name} debe ser de tipo fecha");
                        continue;
                    }

                    _context.FieldValue.Add(new FieldValue
                    {
                        Field = field,
                        DateValue = dateValue,
                        CreatedAt = DateTime.UtcNow,
                        GuestName = request.FilledBy,
                    });
                }
                else if (field.Type == FieldType.Boolean)
                {
                    if (userValue!.Value is not bool booleanValue)
                    {
                        // Add error to ModelState
                        ModelState.AddModelError(field.Name, $"El campo {field.Name} debe ser de tipo booleano");
                        continue;
                    }

                    _context.FieldValue.Add(new FieldValue
                    {
                        Field = field,
                        BooleanValue = booleanValue,
                        CreatedAt = DateTime.UtcNow,
                        GuestName = request.FilledBy,
                    });
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return Ok(new FormFillResponse("Formulario llenado exitosamente", request.FilledBy));
        }

        public record FormFillRequest(string FilledBy, List<FieldRequest> Fields);
        public record FieldRequest(string Name, object? Value);
        public record FormFillResponse(string Message, string Guest);
    }
}
