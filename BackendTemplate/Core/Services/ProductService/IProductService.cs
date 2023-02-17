using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.ProductDto;
using HelpCenter.Models.Product;

namespace HelpCenter.Core.Services.ProductService
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
