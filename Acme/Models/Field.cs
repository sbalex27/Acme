using System.Collections.Generic;
using System.ComponentModel;

namespace Acme.Models
{
    public enum FieldType
    {
        [Description("Text")]
        Text,
        [Description("Number")]
        Number,
        [Description("Date")]
        Date,
        [Description("Boolean")]
        Boolean
    }

    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public FieldType Type { get; set; }
        public int FormId { get; set; }

        public List<FieldValue> Values { get; set; } = [];
        public Form Form { get; set; } = default!;
    }
}
