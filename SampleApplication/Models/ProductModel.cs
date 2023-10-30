namespace SampleApplication.Models
{
    public class ProductModel
    {

        public int Id { get; set; }
        //   [Required]
        public string Name { get; set; }

        //   [StringLength(3, ErrorMessage = "Description must be 3 characters long")]
        public string Description { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


    }
}
