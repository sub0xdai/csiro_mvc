using System;
using System.ComponentModel.DataAnnotations;

namespace csiro_mvc.Models
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Ranking { get; set; }
    }
}
