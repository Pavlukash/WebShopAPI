#nullable enable
using WebShop.Domain.Entities;

namespace WebShop.Domain.Models
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; } 
        
        public string Password { get; set; }
        
        public bool IsBaned { get; set; }
        
        public int RoleId { get; set; }
        
        public RoleEntity Role { get; set; }
        
        public string? PhoneNumber { get; set; } 
    }
}