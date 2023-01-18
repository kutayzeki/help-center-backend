using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Dtos.CompanyDto;
using FeedbackHub.Models.Company;

namespace FeedbackHub.Core.Services.CompanyService
{
    public interface ICompanyService
    {
        Task<PagedApiResponseViewModel<Company>> GetAll(int pageNumber, int pageSize);
        Task<Company> GetById(Guid id);
        Task<ApiResponseViewModel> Create(Create data);
        Task<ApiResponseViewModel> Update(Update data);
        Task<ApiResponseViewModel> Delete(Guid id);
    }
}
