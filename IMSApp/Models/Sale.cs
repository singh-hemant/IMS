using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IMSApp.Models
{
    // Sales Model
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int TotalAmount { get; set; }
    }

}
