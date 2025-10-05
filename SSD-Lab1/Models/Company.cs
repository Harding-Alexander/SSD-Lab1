using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Range(0, 1000)]
        public int YearsInBusiness { get; set; }

        [Required, Url]
        public string Website { get; set; }

        public string Province { get; set; }
    }
}
