using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Dtos.ProductDto;
using FeedbackHub.Models.Product;

namespace FeedbackHub.Core.Services.ProductService
{
    public interface IProductService
    {
        Task<PagedApiResponseViewModel<Product>> GetAll(int pageNumber, int pageSize);
        Task<Product> GetById(Guid id);
        Task<ApiResponseViewModel> Create(ProductCreate data);
        Task<ApiResponseViewModel> Update(ProductUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);
    }
}
