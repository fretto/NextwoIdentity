using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextwoIdentity.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        [Required(ErrorMessage ="name is required")]
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set;}

        [Required(ErrorMessage = "price is required")]

        public decimal? ProductPrice { get; set; } 

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
