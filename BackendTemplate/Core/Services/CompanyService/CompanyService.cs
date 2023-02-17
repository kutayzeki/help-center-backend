using HelpCenter.Models.Company;
using HelpCenter.Models;
using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Controllers;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using HelpCenter.Dtos.CompanyDto;

namespace HelpCenter.Core.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public CompanyService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<Company>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.Companies.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 :(int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.Companies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<Company>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<Company> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.Companies.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No company found with the provided ID");
            }

            return data;
        }

        public async Task<ApiResponseViewModel> Create(Create data)
        {
            //var newAccount = new AccountType
            //{
            //    AccountTypeId = Guid.NewGuid(),
            //    Type = "Standard",
            //    MaxFeatureRequests = 1,
            //    MaxProducts = 1,
            //    MaxUsers = 1,
            //    HasAnalytics = true,
            //    IsActive = true,
            //};
            //_context.AccountTypes.Add(newAccount);
            //await _context.SaveChangesAsync();
            ApiResponseViewModel model = new();

            var accountType = _context.AccountTypes.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name"); 
                return model;
            }
            try
            {
                var newCompany = new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = data.Name,
                    Description = data.Description,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    ImgUrl = null,
                    Theme = "default",
                    IsActive = true,
                    AccountTypeId = accountType.AccountTypeId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Companies.Add(newCompany);
                await _context.SaveChangesAsync();

                model.Id = newCompany.CompanyId.ToString();
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
        public async Task<ApiResponseViewModel> Update(Update data)
        {
            ApiResponseViewModel model = new();
            
            try
            {
                var item = await _context.Companies.AsNoTracking().SingleOrDefaultAsync(x => x.CompanyId == data.CompanyId);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateCompany = new Company
                {
                    CompanyId = item.CompanyId,
                    Name = data.Name ?? data.Name,
                    Description = data.Description ?? data.Description,
                    Email = data.Email ?? data.Email,
                    PhoneNumber = data.PhoneNumber ?? data.PhoneNumber,
                    ImgUrl = data.ImgUrl ?? data.PhoneNumber,
                    Theme = data.Theme ?? data.Theme,
                    IsActive = item.IsActive,
                    AccountTypeId = item.AccountTypeId
                };

                _context.Companies.Update(updateCompany);
                await _context.SaveChangesAsync();

                model.Id = item.CompanyId.ToString();
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
                var item = await _context.Companies.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.Companies.Remove(item);
                await _context.SaveChangesAsync();

                model.Id = item.CompanyId.ToString();
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
