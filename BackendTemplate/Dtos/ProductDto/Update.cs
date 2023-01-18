
namespace FeedbackHub.Dtos.ProductDto
{
    public class ProductUpdate
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
    }
}
