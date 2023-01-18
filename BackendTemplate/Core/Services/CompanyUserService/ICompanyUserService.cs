using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Dtos.CompanyUserDto;
using FeedbackHub.Models.CompanyUser;

namespace FeedbackHub.Core.Services.CompanyUserService
{
    public interface ICompanyUserService
    {
        Task<ApiResponseViewModel> Create(CompanyUserCreate data);
        Task<ApiResponseViewModel> Delete(Guid companyId, string userId);
    }
}
