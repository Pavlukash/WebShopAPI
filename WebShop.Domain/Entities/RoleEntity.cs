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
        
        [Required] 
        public bool CanApplyDiscounts { get; set; }
        [Required] 
        public bool CanEditProducts { get; set; }
        [Required] 
        public bool CanEditRoles { get; set; }
        [Required] 
        public bool CanBanUsers { get; set; }
        
        public List<ClientEntity> Users { get; set; }
    }
}