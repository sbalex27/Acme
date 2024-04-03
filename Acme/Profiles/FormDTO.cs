using Acme.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Acme.Profiles
{
    class FormProfile : Profile
    {
        public FormProfile()
        {
            CreateMap<Form, FormDTO>().ReverseMap();
            CreateMap<Field, FieldDTO>().ReverseMap().ForMember(dest => dest.Values, opt => opt.Ignore());
        }
    }

    public class FormDTO
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public string? Link { get; set; }
        [Required]
        public IEnumerable<FieldDTO> Fields { get; set; } = null!;
    }

    public class FieldDTO
    {
        public int? Id { get; set; } = null;
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
