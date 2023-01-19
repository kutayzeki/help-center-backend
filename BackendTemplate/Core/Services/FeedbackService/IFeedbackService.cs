using FeedbackHub.Core.Helpers.ResponseModels;
using FeedbackHub.Dtos.FeedbackDto;
using FeedbackHub.Models.Feedback;

namespace FeedbackHub.Core.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<PagedApiResponseViewModel<Feedback>> GetAll(int pageNumber, int pageSize);
        Task<Feedback> GetById(Guid id);
        Task<ApiResponseViewModel> Create(FeedbackCreate data);
        Task<ApiResponseViewModel> Update(FeedbackUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);
        Task<ApiResponseViewModel> CreateFeedbackUpvote(CreateUpvote data);
        Task<ApiResponseViewModel> DeleteFeedbackUpvote(Guid id);

    }
}
