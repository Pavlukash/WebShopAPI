using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        
        [Required]
        [JsonIgnore]
        public string Password { get; set; } = null!;
        
        [MaxLength(20)]
        public string? PhoneNumber { get; set; } 
        
        [Required]
        public bool IsBaned { get; set; }

        [Required] 
        public int RoleId { get; set; }
        
        [Required]
        public RoleEntity Role { get; set; } = null!;
        
        public IEnumerable<ClientsProductsEntity>? ClientsProductsEntities { get; set; }
        
        public IEnumerable<ClientsDiscountsEntity>? ClientsDiscountsEntities { get; set; }

        public IEnumerable<OrderEntity>? Orders { get; set; }
        
        
    }
}