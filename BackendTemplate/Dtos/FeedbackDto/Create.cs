using FeedbackHub.Models.Feedback;

namespace FeedbackHub.Dtos.FeedbackDto
{
    public class FeedbackCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FeedbackType Type { get; set; }
        public Guid ProductId { get; set; }
        public string UserId { get; set; }

    }
}
