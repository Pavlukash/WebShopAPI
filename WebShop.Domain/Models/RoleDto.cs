namespace WebShop.Domain.Models
{
    public class RoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public bool CanApplyDiscounts { get; set; }
        
        public bool CanEditProducts { get; set; }
       
        public bool CanEditRoles { get; set; }
 
        public bool CanBanUsers { get; set; }
    }
}