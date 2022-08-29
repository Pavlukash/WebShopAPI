using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Domain.Entities
{
    public class RoleEntity
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        [MaxLength(20)] 
        public string Name { get; set; } = null!;
        
        public List<ClientEntity> Users { get; set; } = null!;
    }
}