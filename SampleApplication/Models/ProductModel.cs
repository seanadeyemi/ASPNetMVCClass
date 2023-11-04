using System.ComponentModel.DataAnnotations;

namespace SampleApplication.Models
{
    public class ProductModel
    {

        public int Id { get; set; } = 0;
        public string Name { get; set; }
        //   [StringLength(3, ErrorMessage = "Description must be 3 characters long")]
        public string Description { get; set; }

        public string Color { get; set; }
        [Range(0, 500)]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


    }
}
