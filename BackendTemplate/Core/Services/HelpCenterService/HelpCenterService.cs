using HelpCenter.Controllers;
using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.HelpCenterDto;
using HelpCenter.Models;
using HelpCenter.Models.HelpCenter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HelpCenter.Core.Services.HelpCenterService
{
    public class HelpCenterService : IHelpCenterService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public HelpCenterService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<HelpCenterModel>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.HelpCenters.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.HelpCenters
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<HelpCenterModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<HelpCenterModel> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.HelpCenters.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No HelpCenter found with the provided ID");
            }

            return data;
        }
        public async Task<PagedApiResponseViewModel<GetHelpCenters>> GetHelpCentersByProductId(Guid productId, int pageNumber, int pageSize)
        {
            var isProductExist = await _context.Products.FindAsync(productId);

            if (isProductExist == null)
            {
                throw new ArgumentException("Invalid Product ID provided");

            }
           
            var feedbacks = from f in _context.HelpCenters
                            join p in _context.Products on f.ProductId equals p.ProductId
                            where p.ProductId == productId
                            select new GetHelpCenters
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Icon = f.Icon,
                                Color= f.Color,
                                Position= f.Position,
                                ProductName = p.Name,
                                IsActive= f.IsActive,
                            };
            
            // Retrieve the total number of products
            var totalRecords = feedbacks.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = feedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<GetHelpCenters>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<ApiResponseViewModel> Create(HelpCenterCreate data)
        {

            ApiResponseViewModel model = new();

            #region validate

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name");
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.Icon.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Icon");
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.ProductId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "ProductId");
                return model;
            }
            
            #endregion


            try
            {
                var product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.ProductId == data.ProductId);
                if (product == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "Product");
                    return model;
                }
               
                var newHelpCenter = new HelpCenterModel
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Icon = data.Icon,
                    Color= data.Color,
                    Position= data.Position,
                    IsActive = true,
                    ProductId = data.ProductId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.HelpCenters.Add(newHelpCenter);
                await _context.SaveChangesAsync();

                model.Id = newHelpCenter.Id.ToString();
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
        public async Task<ApiResponseViewModel> Update(HelpCenterUpdate data)
        {
            ApiResponseViewModel model = new();

            try
            {
                var item = await _context.HelpCenters.AsNoTracking().SingleOrDefaultAsync(x => x.Id == data.Id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateHelpCenter = new HelpCenterModel
                {
                    Id = item.Id,
                    Name = data.Name,
                    Icon = data.Icon,
                    Color = data.Color,
                    Position = data.Position,
                    ProductId = item.ProductId,
                    IsActive = data.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.HelpCenters.Update(updateHelpCenter);
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
                var item = await _context.HelpCenters.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.HelpCenters.Remove(item);
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
