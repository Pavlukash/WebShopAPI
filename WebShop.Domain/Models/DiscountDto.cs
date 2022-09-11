namespace WebShop.Domain.Models
{
    public class DiscountDto
    {
        public int Id { get; set; }
        
        public int ClientId { get; set; }

        public int ProductId { get; set; }
        
        public decimal Discount { get; set; }
    }
}