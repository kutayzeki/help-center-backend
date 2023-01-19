using FeedbackHub.Models.User;

namespace FeedbackHub.Models.Feedback
{
    public class FeedbackUpvote
    {
        public Guid Id { get; set; }
        public DateTime UpvoteDateTime { get; set; }
        public Guid FeedbackId { get; set; }
        public virtual Feedback Feedback{ get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User{ get; set; }

    }
}
