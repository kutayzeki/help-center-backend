using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.HelpCenterDto;
using HelpCenter.Models.HelpCenter;

namespace HelpCenter.Core.Services.HelpCenterService
{
    public interface IHelpCenterService
    {
        Task<PagedApiResponseViewModel<HelpCenterModel>> GetAll(int pageNumber, int pageSize);
        Task<HelpCenterModel> GetById(Guid id);
        Task<PagedApiResponseViewModel<GetHelpCenters>> GetHelpCentersByProductId(Guid productId, int pageNumber, int pageSize);
        Task<ApiResponseViewModel> Create(HelpCenterCreate data);
        Task<ApiResponseViewModel> Update(HelpCenterUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);

    }
}
