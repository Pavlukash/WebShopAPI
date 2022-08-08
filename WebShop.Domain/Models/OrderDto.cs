namespace WebShop.Domain.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}