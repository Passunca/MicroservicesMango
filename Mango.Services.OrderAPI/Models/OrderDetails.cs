using Mango.Services.OrderAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }

        [ForeignKey("OrderHeader")]
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }

        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }

        [NotMapped]
        public OrderHeader? OrderHeader { get; set; }

        [NotMapped]
        public ProductDto? Product { get; set; }
    }
}
