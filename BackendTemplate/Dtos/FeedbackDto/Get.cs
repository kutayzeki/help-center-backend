using FeedbackHub.Models.Feedback;
using FeedbackHub.Models.Product;

namespace FeedbackHub.Dtos.FeedbackDto
{
    public class GetFeedbacks
    {
        public Guid FeedbackId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
    }
}
