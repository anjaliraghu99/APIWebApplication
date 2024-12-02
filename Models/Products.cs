using System.ComponentModel.DataAnnotations;

namespace APIWebApplication.Models
{
    public class Products
    {
        [Key]
        public string? Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int Price { get; set; }
    }
}
