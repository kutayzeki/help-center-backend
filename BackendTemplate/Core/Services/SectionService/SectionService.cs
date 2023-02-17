using HelpCenter.Controllers;
using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.SectionDto;
using HelpCenter.Models;
using HelpCenter.Models.Section;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HelpCenter.Core.Services.SectionService
{
    public class SectionService : ISectionService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public SectionService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<Section>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.Sections.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.Sections
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<Section>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<Section> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.Sections.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No Section found with the provided ID");
            }

            return data;
        }
        public async Task<PagedApiResponseViewModel<GetSections>> GetSectionsByHelpCenterId(Guid helpCenterId,int pageNumber, int pageSize)
        {
            var isHelpCenterExists = await _context.HelpCenters.FindAsync(helpCenterId);

            if (isHelpCenterExists == null)
            {
                throw new ArgumentException("Invalid Product ID provided");

            }
           
            var Sections = from f in _context.Sections
                            join p in _context.HelpCenters on f.HelpCenterId equals p.Id
                            where p.Id == helpCenterId
                            select new GetSections
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Size = f.Size,
                                Order = f.Order,
                                HelpCenterName = p.Name,
                                IsActive = true,
                            };
                       
            // Retrieve the total number of products
            var totalRecords = Sections.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = Sections
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<GetSections>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<ApiResponseViewModel> Create(SectionCreate data)
        {

            ApiResponseViewModel model = new();

            #region validate

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name");
                return model;
            }
           
            if (string.IsNullOrWhiteSpace(data.HelpCenterId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "HelpCenterId");
                return model;
            }
         
            #endregion
           
            try
            {
                var helpCenter = await _context.HelpCenters.AsNoTracking().SingleOrDefaultAsync(x => x.Id == data.HelpCenterId);
                if (helpCenter == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "Company");
                    return model;
                }
               
                var newSection = new Section
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Size = data.Size,
                    HelpCenterId = data.HelpCenterId,
                    IsActive = true,
                    Order= data.Order,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Sections.Add(newSection);
                await _context.SaveChangesAsync();

                model.Id = newSection.Id.ToString();
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
        public async Task<ApiResponseViewModel> Update(SectionUpdate data)
        {
            ApiResponseViewModel model = new();

            try
            {
                var item = await _context.Sections.AsNoTracking().SingleOrDefaultAsync(x => x.Id == data.Id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateSection = new Section
                {
                    Id = item.Id,
                    Name = item.Name,
                    Size= item.Size,
                    HelpCenterId= item.HelpCenterId,
                    Order= item.Order,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Sections.Update(updateSection);
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
                var item = await _context.Sections.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.Sections.Remove(item);
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
