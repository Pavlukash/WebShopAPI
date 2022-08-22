using System.ComponentModel.DataAnnotations;

namespace WebShop.Common.Requests
{
    public class LoginModel
    {
        [Required] 
        [MaxLength(20)] 
        public string EmailForLogin { get; set; }
 
        [Required] 
        [MaxLength(20)] 
        public string PasswordForLogin { get; set; }
        
        [Required] 
        [MaxLength(20)] 
        public string RoleForLogin { get; set; }
    }
}