namespace Acme.Models
{
    public class FieldValue
    {
        public int Id { get; set; }
        public Field Field { get; set; } = null!;
        public string? TextValue { get; set; }
        public int? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // TODO: Add user property
    }
}
