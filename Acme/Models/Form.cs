﻿namespace Acme.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public List<Field> Fields { get; set; } = [];
    }
}
