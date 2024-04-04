using System.ComponentModel.DataAnnotations;

namespace Acme.Http.Requests
{
    public class FormRequest
    {
        [Required]
        public string FormName { get; set; } = null!;
        [Required]
        public string FormDescription { get; set; } = null!;
        [Required]
        public List<FieldDTO> Fields { get; set; } = null!;
    }
}

