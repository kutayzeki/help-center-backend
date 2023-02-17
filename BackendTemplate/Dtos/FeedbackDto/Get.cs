using HelpCenter.Models.Feedback;
using HelpCenter.Models.Product;

namespace HelpCenter.Dtos.FeedbackDto
{
    public class GetFeedbacks
    {
        public Guid FeedbackId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
    }
}
