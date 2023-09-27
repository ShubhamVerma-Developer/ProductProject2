using System.ComponentModel.DataAnnotations;

namespace SHUBHAM_PRODUCT.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Name")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Name can not contain numbers or special characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Select a Category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter the quantity.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please Choose the type.")]
        public string Type { get; set; }

        public string? Image { get; set; }
        public string? CategoryName { get; set; }
    }

    public class Item
    {
        public int CatId { get; set; }
        public string PName { get; set; }
    }
}
