using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.CompanyUserDto;
using HelpCenter.Models.CompanyUser;

namespace HelpCenter.Core.Services.CompanyUserService
{
    public interface ICompanyUserService
    {
        Task<ApiResponseViewModel> Create(CompanyUserCreate data);
        Task<ApiResponseViewModel> Delete(Guid companyId, string userId);
    }
}
