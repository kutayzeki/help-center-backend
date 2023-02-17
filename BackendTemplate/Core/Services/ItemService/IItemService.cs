using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.ItemDto;
using HelpCenter.Models.Item;
using Item.Dtos.ItemDto;

namespace HelpCenter.Core.Services.ItemService
{
    public interface IItemService
    {
        Task<PagedApiResponseViewModel<ItemModel>> GetAll(int pageNumber, int pageSize);
        Task<ItemModel> GetById(Guid id);
        Task<PagedApiResponseViewModel<GetItems>> GetItemsBySectionId(Guid sectionId, int pageNumber, int pageSize);
        Task<ApiResponseViewModel> Create(ItemCreate data);
        Task<ApiResponseViewModel> Update(ItemUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);

    }
}
