using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

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
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "Please select a category")] // Add Required attribute
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")] // Use Range to ensure the value is greater than 0
        public int SelectedCategoryId { get; set; } // Property to hold the selected category ID
        public IEnumerable<SelectListItem> Categories { get; set; } // List of available categories

        public List<string> ImagePaths { get; set; }
        [DisplayName("Images")]
        public List<HttpPostedFileBase> Images { get; set; } // Property for image uploads

    }
}
