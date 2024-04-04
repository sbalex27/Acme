using Acme.Models;
using System.ComponentModel.DataAnnotations;

namespace Acme.Http.Requests
{
    public class FieldDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public FieldType Type { get; set; }
    }
}

