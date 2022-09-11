using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Domain.Entities
{
    public class ProductEntity
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)] 
        public string Name { get; set; } = null!;

        [Required] 
        [MaxLength(500)] 
        public string Description { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? Price { get; set; }

        public IEnumerable<ClientsProductsEntity> ClientsProductsEntities { get; set; }
        
        public List<OrderEntity>? OrderList { get; set; }

        public List<ClientEntity>? ClientList { get; set; }
    }
}