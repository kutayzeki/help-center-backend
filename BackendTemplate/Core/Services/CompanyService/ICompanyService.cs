using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.CompanyDto;
using HelpCenter.Models.Company;

namespace HelpCenter.Core.Services.CompanyService
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
