using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class ClientEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required] 
        [MaxLength(20)] 
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = null!;
        
        [MaxLength(20)]
        public string? PhoneNumber { get; set; } 
        
        public List<OrderEntity> OrderList { get; set; }
    }
}