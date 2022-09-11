using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class ClientsDiscountEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        public int ClientId { get; set; }
        
        [Required] 
        public int DiscountId { get; set; }
        
        public ClientEntity ClientEntity { get; set; }  = null!;
        
        public DiscountEntity DiscountEntity { get; set; }  = null!;
    }
}