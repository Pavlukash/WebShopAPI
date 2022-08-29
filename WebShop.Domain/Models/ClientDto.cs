#nullable enable
using WebShop.Domain.Entities;

namespace WebShop.Domain.Models
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool IsBaned { get; set; }
        
        public int RoleId { get; set; }
        
        public string PhoneNumber { get; set; } = null!;
    }
}