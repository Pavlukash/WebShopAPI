using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class DiscountEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        public int ProductId { get; set; }
        
        [Required]
        public decimal Discount { get; set; }
        
        public ProductEntity? Product { get; set; }

        public IEnumerable<ClientsDiscountsEntity>? ClientsDiscountsEntities { get; set; }
    }
}