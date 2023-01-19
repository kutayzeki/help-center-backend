using FeedbackHub.Controllers;
using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Dtos.FeedbackDto;
using FeedbackHub.Models;
using FeedbackHub.Models.Feedback;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FeedbackHub.Core.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly APIDbContext _context;
        private readonly IStringLocalizer<LocalizerController> _stringLocalizer;

        public FeedbackService(APIDbContext context, IStringLocalizer<LocalizerController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<PagedApiResponseViewModel<Feedback>> GetAll(int pageNumber, int pageSize)
        {
            // Retrieve the total number of products
            var totalRecords = _context.Feedbacks.Count();
            // Calculate the total number of pages
            var totalPages = pageSize == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / pageSize);

            // Retrieve the paginated products
            var data = _context.Feedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model
            var model = new PagedApiResponseViewModel<Feedback>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<Feedback> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID provided");
            }

            var data = await _context.Feedbacks.FindAsync(id);

            if (data == null)
            {
                throw new ArgumentException("No Feedback found with the provided ID");
            }

            return data;
        }
        public async Task<PagedApiResponseViewModel<GetFeedbacks>> GetFeedbacksByProductId(Guid productId, FeedbackType feedbackType, int pageNumber, int pageSize)
        {
            var isProductExist = await _context.Products.FindAsync(productId);

            if (isProductExist == null)
            {
                throw new ArgumentException("Invalid Product ID provided");

            }
            if (feedbackType != FeedbackType.FeatureRequest && feedbackType != FeedbackType.Idea && feedbackType != FeedbackType.Bug && feedbackType != FeedbackType.All)
            {
                throw new Exception("Invalid feedback type");

            }
            var feedbacks = from f in _context.Feedbacks
                            join p in _context.Products on f.ProductId equals p.ProductId
                            where p.ProductId == productId
                            select new GetFeedbacks
                            {
                                FeedbackId = f.FeedbackId,
                                Title = f.Title,
                                Description= f.Description,
                                FeedbackType = f.Type,
                                Upvotes = f.FeedbackUpvotes.Count,
                                ProductName = p.Name,
                                CompanyName = p.Company.Name
                            };
            if (feedbackType != FeedbackType.All)
            {
                feedbacks = feedbacks.Where(x => x.FeedbackType == feedbackType);
            }
            
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
            var model = new PagedApiResponseViewModel<GetFeedbacks>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = data
            };

            return model;
        }

        public async Task<ApiResponseViewModel> Create(FeedbackCreate data)
        {

            ApiResponseViewModel model = new();

            #region validate

            if (string.IsNullOrWhiteSpace(data.Title))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Name");
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.Type.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "Type");
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.ProductId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "ProductId");
                return model;
            }
            if (string.IsNullOrWhiteSpace(data.UserId.ToString()))
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Required"], "UserId");
                return model;
            }
            #endregion
            if (data.Type != FeedbackType.FeatureRequest && data.Type != FeedbackType.Idea && data.Type != FeedbackType.Bug)
            {
                model.IsSuccess = false;
                model.Message = string.Format(_stringLocalizer["Invalid"], "Feedback type");
                return model;
            }
            try
            {
                var product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.ProductId == data.ProductId);
                var user = await _context.Users.FindAsync(data.UserId);
                if (product == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "Company");
                    return model;
                }
                if (user == null)
                {
                    model.IsSuccess = false;
                    model.Message = string.Format(_stringLocalizer["Invalid"], "User");
                    return model;
                }
                var newFeedback = new Feedback
                {
                    FeedbackId = Guid.NewGuid(),
                    Title = data.Title,
                    Description = data.Description,
                    Type = data.Type,
                    IsActive = true,
                    ProductId = data.ProductId,
                    UserId = data.UserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Feedbacks.Add(newFeedback);
                await _context.SaveChangesAsync();

                model.Id = newFeedback.FeedbackId.ToString();
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
        public async Task<ApiResponseViewModel> Update(FeedbackUpdate data)
        {
            ApiResponseViewModel model = new();

            try
            {
                var item = await _context.Feedbacks.AsNoTracking().SingleOrDefaultAsync(x => x.FeedbackId == data.FeedbackId);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }

                var updateFeedback = new Feedback
                {
                    FeedbackId = item.FeedbackId,
                    Title = data.Title,
                    Description = data.Description,
                    Type = data.Type,
                    ProductId = item.ProductId,
                    UserId = item.UserId,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Feedbacks.Update(updateFeedback);
                await _context.SaveChangesAsync();

                model.Id = item.FeedbackId.ToString();
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
                var item = await _context.Feedbacks.FindAsync(id);
                if (item == null)
                {
                    model.IsSuccess = false;
                    model.Message = _stringLocalizer["NotFound"].ToString();
                    return model;
                }
                _context.Feedbacks.Remove(item);
                await _context.SaveChangesAsync();

                model.Id = item.FeedbackId.ToString();
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

        public async Task<ApiResponseViewModel> CreateFeedbackUpvote(CreateUpvote data)
        {
            ApiResponseViewModel model = new();

            var alreadyExists = _context.FeedbackUpvotes.Any(x => x.FeedbackId == data.FeedbackId && x.UserId == data.UserId);
            if (alreadyExists)
            {
                model.IsSuccess = false;
                model.Message = _stringLocalizer["AlreadyExist"].ToString();
                return model;
            }
            try
            {
                var upvote = new FeedbackUpvote
                {
                    Id = Guid.NewGuid(),
                    FeedbackId = data.FeedbackId,
                    UserId = data.UserId,
                    UpvoteDateTime = DateTime.Now
                };
                _context.FeedbackUpvotes.Add(upvote);
                _context.SaveChanges();

                model.Id = data.FeedbackId.ToString();
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
        public async Task<ApiResponseViewModel> DeleteFeedbackUpvote(Guid id)
        {
            ApiResponseViewModel model = new();
            try
            {
                    var item = await _context.FeedbackUpvotes.FindAsync(id);
                    if (item == null)
                    {
                        model.IsSuccess = false;
                        model.Message = _stringLocalizer["NotFound"].ToString();
                        return model;
                    }

                    _context.FeedbackUpvotes.Remove(item);
                    await _context.SaveChangesAsync();

                    model.Id = id.ToString();
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
