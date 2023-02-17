using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models.Product
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company.Company Company { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Feedback.Feedback> Feedbacks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
