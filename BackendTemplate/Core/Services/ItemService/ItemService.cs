using HelpCenter.Controllers;
using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Core.Services.ItemService;
using HelpCenter.Dtos.ItemDto;
using HelpCenter.Models;
using HelpCenter.Models.Item;
using Item.Dtos.ItemDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HelpCenter.Core.Services.ItemService
{
    public class ItemService : IItemService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public ItemService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<ItemModel>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.Items.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.Items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<ItemModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<ItemModel> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.Items.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No Item found with the provided ID");
            }

            return data;
        }
        public async Task<PagedApiResponseViewModel<GetItems>> GetItemsBySectionId(Guid SectionId,int pageNumber, int pageSize)
        {
            var isSectionExists = await _context.Sections.FindAsync(SectionId);

            if (isSectionExists == null)
            {
                throw new ArgumentException("Invalid Product ID provided");

            }
           
            var Items = from f in _context.Items
                            join p in _context.Sections on f.SectionId equals p.Id
                            where p.Id == SectionId
                            select new GetItems
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Type= f.Type,
                                Url= f.Url,
                                Order = f.Order,
                                SectionName = p.Name,
                                IsActive = f.IsActive,
                            };
                       
            // Retrieve the total number of products
            var totalRecords = Items.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = Items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<GetItems>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<ApiResponseViewModel> Create(ItemCreate data)
        {

            ApiResponseViewModel model = new();

            #region validate

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name");
                return model;
            }
           
            if (string.IsNullOrWhiteSpace(data.SectionId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "SectionId");
                return model;
            }
         
            #endregion
           
            try
            {
                var Section = await _context.Sections.AsNoTracking().SingleOrDefaultAsync(x => x.Id == data.SectionId);
                if (Section == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "Section");
                    return model;
                }
               
                var newItem = new ItemModel
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Type= data.Type,
                    SectionId = data.SectionId,
                    IsActive = data.IsActive,
                    Url = data.Url,
                    Order= data.Order,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Items.Add(newItem);
                await _context.SaveChangesAsync();

                model.Id = newItem.Id.ToString();
                model.IsSuccess = true;
                model.Message = _stringLocalizer["ResourceCreated"].ToString();

            }
            catch (Exception e)
            {
                model.IsSuccess = false;
                model.Message = e.ToString();
            }
            return model;
        }
        public async Task<ApiResponseViewModel> Update(ItemUpdate data)
        {
            ApiResponseViewModel model = new();

            try
            {
                var item = await _context.Items.AsNoTracking().SingleOrDefaultAsync(x => x.Id == data.Id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateItem = new ItemModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type= data.Type,
                    SectionId= item.SectionId,
                    Order= item.Order,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Items.Update(updateItem);
                await _context.SaveChangesAsync();

                model.Id = item.Id.ToString();
                model.IsSuccess = true;
                model.Message = _stringLocalizer["ResourceUpdated"].ToString();

            }
            catch (Exception e)
            {
                model.IsSuccess = false;
                model.Message = e.ToString();
            }
            return model;
        }

        public async Task<ApiResponseViewModel> Delete(Guid id)
        {
            ApiResponseViewModel model = new();
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                model.Id = item.Id.ToString();
                model.IsSuccess = true;
                model.Message = _stringLocalizer["ResourceDeleted"].ToString();
            }
            catch (Exception e)
            {
                model.IsSuccess = false;
                model.Message = e.ToString();
            }
            return model;
        }
    }
}
