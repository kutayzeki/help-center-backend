﻿using HelpCenter.Core.Helpers.ResponseModels;
using HelpCenter.Dtos.FeedbackDto;
using HelpCenter.Models.Feedback;

namespace HelpCenter.Core.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<PagedApiResponseViewModel<Feedback>> GetAll(int pageNumber, int pageSize);
        Task<Feedback> GetById(Guid id);
        Task<PagedApiResponseViewModel<GetFeedbacks>> GetFeedbacksByProductId(Guid companyId, FeedbackType feedbackType, int pageNumber, int pageSize);
        Task<ApiResponseViewModel> Create(FeedbackCreate data);
        Task<ApiResponseViewModel> Update(FeedbackUpdate data);
        Task<ApiResponseViewModel> Delete(Guid id);
        Task<ApiResponseViewModel> CreateFeedbackUpvote(CreateUpvote data);
        Task<ApiResponseViewModel> DeleteFeedbackUpvote(Guid id);

    }
}
