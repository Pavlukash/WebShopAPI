using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Domain.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? TotalPrice { get; set; }
        
        public IEnumerable<ClientsOrdersEntity>? ClientsOrdersEntities { get; set; }
        
        public List<ProductEntity>? ProductList { get; set; }
    }
}