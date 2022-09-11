using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Domain.Entities
{
    public class DiscountEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        public int ClientId { get; set; } 
        
        [Required] 
        public int ProductId { get; set; }
        
        [Required] 
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Discount { get; set; }

        public IEnumerable<ClientsDiscountEntity>? ClientsDiscountEntities { get; set; }
    }
}