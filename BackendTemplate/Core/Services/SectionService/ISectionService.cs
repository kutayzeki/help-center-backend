using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.SectionDto;
using HelpCenter.Models.Section;

namespace HelpCenter.Core.Services.SectionService
{
    public interface ISectionService
    {
        Task<PagedApiResponseViewModel<Section>> GetAll(int pageNumber, int pageSize);
        Task<Section> GetById(Guid id);
        Task<PagedApiResponseViewModel<GetSections>> GetSectionsByHelpCenterId(Guid helpCenterId, int pageNumber, int pageSize);
        Task<ApiResponseViewModel> Create(SectionCreate data);
        Task<ApiResponseViewModel> Update(SectionUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);

    }
}
