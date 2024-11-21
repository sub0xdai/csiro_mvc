using System.ComponentModel.DataAnnotations;

namespace csiro_mvc.Models
{
    public class GlobalSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
