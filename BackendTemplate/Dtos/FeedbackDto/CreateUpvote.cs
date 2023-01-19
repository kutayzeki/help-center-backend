namespace FeedbackHub.Dtos.FeedbackDto
{
    public class CreateUpvote
    {
        public Guid FeedbackId { get; set; }
        public string UserId { get; set; }
    }
}
