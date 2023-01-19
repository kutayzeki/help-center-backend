using FeedbackHub.Models.User;
using FeedbackHub.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace FeedbackHub.Models.Feedback
{
    public class Feedback
    {
        public Guid FeedbackId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public virtual Product.Product Product { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }        
        public DateTime? UpdatedAt { get; set; }
        public virtual List<FeedbackUpvote> FeedbackUpvotes { get; set; }

    }
}
