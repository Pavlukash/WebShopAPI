using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class ClientsOrdersEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        public int ClientId { get; set; }
        
        [Required] 
        public int OrderId { get; set; }
        
        public ClientEntity ClientEntity { get; set; }  = null!;
        
        public OrderEntity OrderEntity { get; set; }  = null!;
    }
}