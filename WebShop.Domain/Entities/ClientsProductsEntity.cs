using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class ClientsProductsEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        public int ClientId { get; set; }
        
        [Required] 
        public int ProductId { get; set; }
        
        public ClientEntity ClientEntity { get; set; }  = null!;
        
        public ProductEntity ProductEntity { get; set; }  = null!;
    }
}