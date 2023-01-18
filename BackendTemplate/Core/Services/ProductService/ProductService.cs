using FeedbackHub.Models.Product;
using FeedbackHub.Models;
using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Controllers;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using FeedbackHub.Dtos.ProductDto;

namespace FeedbackHub.Core.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public ProductService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<Product>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.Products.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 :(int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<Product>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<Product> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.Products.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No Product found with the provided ID");
            }

            return data;
        }

        public async Task<ApiResponseViewModel> Create(ProductCreate data)
        {

            ApiResponseViewModel model = new();

            if (string.IsNullOrWhiteSpace(data.Name))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name"); 
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
                var newProduct = new Product
                {
                    ProductId = Guid.NewGuid(),
                    Name = data.Name,
                    CompanyId = data.CompanyId,
                    IsActive = true,
                    CreatedAt= DateTime.UtcNow,
                    UpdatedAt= DateTime.UtcNow,
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                model.Id = newProduct.ProductId.ToString();
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
        public async Task<ApiResponseViewModel> Update(ProductUpdate data)
        {
            ApiResponseViewModel model = new();
            
            try
            {
                var item = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.ProductId == data.ProductId);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateProduct = new Product
                {
                    ProductId = item.ProductId,
                    Name = data.Name ?? data.Name,
                    CompanyId = item.CompanyId,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Products.Update(updateProduct);
                await _context.SaveChangesAsync();

                model.Id = item.ProductId.ToString();
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
                var item = await _context.Products.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.Products.Remove(item);
                await _context.SaveChangesAsync();

                model.Id = item.ProductId.ToString();
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
