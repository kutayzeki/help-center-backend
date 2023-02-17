
using HelpCenter.Models.Feedback;
using HelpCenter.Models.Product;
using HelpCenter.Models.User;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Dtos.FeedbackDto
{
    public class FeedbackUpdate
    {
        public Guid FeedbackId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public FeedbackType Type { get; set; }
    }
}
