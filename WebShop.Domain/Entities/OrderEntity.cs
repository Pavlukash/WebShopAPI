using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public decimal? TotalPrice { get; set; }
        
        public ClientEntity? Client { get; set; }
        public List<ProductEntity>? ProductList { get; set; }
    }
}