using HelpCenter.Models.HelpCenter;
using HelpCenter.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models.Section
{
    public class Section
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public Size Size{ get; set; }
        [Required]
        public Guid HelpCenterId { get; set; }
        public virtual HelpCenter.HelpCenter HelpCenter { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
    public enum Size
    {
        Question = 100,
        Information = 200,
        Academic = 300,
        Message = 400
    }
    public enum Position
    {
        BottomLeft = 100,
        BottomRight = 200,
    }

}
