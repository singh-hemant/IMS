using System.ComponentModel.DataAnnotations;

namespace IMSApp.Models
{
    // Suppliers Model
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ContactInfo { get; set; }

        public string Address { get; set; }
    }
}
