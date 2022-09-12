using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Domain.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public decimal? TotalPrice { get; set; }

        public ClientEntity? Client { get; set; }
    }
}