using FeedbackHub.Models.Company;
using FeedbackHub.Models;
using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Models.Product;

namespace FeedbackHub.Core.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly APIDbContext _context;

        public CompanyService(APIDbContext context)
        {
            _context = context;
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

        public async Task Create(string name, string description, string email, string phoneNumber)
        {
            var newAccount = new AccountType
            {
                AccountTypeId = Guid.NewGuid(),
                Type = "Standard",
                MaxFeatureRequests = 1,
                MaxProducts = 1,
                MaxUsers = 1,
                HasAnalytics = true,
                IsActive = true,
            };
            _context.AccountTypes.Add(newAccount);
            await _context.SaveChangesAsync();
            var newCompany = new Company
            {
                CompanyId  = Guid.NewGuid(),
                Name = name,
                Description = description,
                Email = email,
                PhoneNumber = phoneNumber,
                ImgUrl = null,
                Theme = "default",
                IsActive = true,
                AccountTypeId = newAccount.AccountTypeId
            };

            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
