using HelpCenter.Models.CompanyUser;
using HelpCenter.Models;
using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Controllers;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using HelpCenter.Dtos.CompanyUserDto;

namespace HelpCenter.Core.Services.CompanyUserService
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public CompanyUserService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }

       
        public async Task<ApiResponseViewModel> Create(CompanyUserCreate data)
        {

            ApiResponseViewModel model = new();

            if (string.IsNullOrWhiteSpace(data.UserId))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "UserId"); 
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.CompanyId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "CompanyId");
                return model;
            }
            try
            {
                var company = await _context.Companies.FindAsync(data.CompanyId);
                if (company == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "Company");
                    return model;
                }
                var user = await _context.Users.FindAsync(data.UserId);
                if (user == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "User");
                    return model;
                }
                var newCompanyUser = new CompanyUser
                {

                    CompanyId = data.CompanyId,
                    UserId = data.UserId,
                    CreatedAt= DateTime.UtcNow,
                    UpdatedAt= DateTime.UtcNow,
                };

                _context.CompanyUsers.Add(newCompanyUser);
                await _context.SaveChangesAsync();

                model.Id = newCompanyUser.CompanyId.ToString();
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
        
        public async Task<ApiResponseViewModel> Delete(Guid companyId, string userId)
        {
            ApiResponseViewModel model = new();
            try
            {
                var item = await _context.CompanyUsers.Where(x => x.CompanyId == companyId && x.UserId == userId).FirstOrDefaultAsync();
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.CompanyUsers.Remove(item);
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
