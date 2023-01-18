
using FeedbackHub.Models.Product;
using FeedbackHub.Models.User;
using System.ComponentModel.DataAnnotations;

namespace FeedbackHub.Dtos.FeedbackDto
{
    public class FeedbackUpdate
    {
        public Guid FeedbackId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }
}
