using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.SectionDto;
using HelpCenter.Models.Section;

namespace HelpCenter.Core.Services.SectionService
{
    public interface ISectionService
    {
        Task<PagedApiResponseViewModel<SectionModel>> GetAll(int pageNumber, int pageSize);
        Task<SectionModel> GetById(Guid id);
        Task<PagedApiResponseViewModel<GetSections>> GetSectionsByHelpCenterId(Guid helpCenterId, int pageNumber, int pageSize);
        Task<ApiResponseViewModel> Create(SectionCreate data);
        Task<ApiResponseViewModel> Update(SectionUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);

    }
}
