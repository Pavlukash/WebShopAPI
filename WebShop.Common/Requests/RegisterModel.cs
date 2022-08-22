using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Requests
{
    public class RegisterModel
    {
        [Required] 
        [MaxLength(20)] 
        public string Email { get; set; }
 
        [Required] 
        [MaxLength(20)] 
        public string Password { get; set; }
 
        [Required] 
        [MaxLength(20)] 
        public string ConfirmPassword { get; set; }
    }
}