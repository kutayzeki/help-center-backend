using HelpCenter.Models.Feedback;
using HelpCenter.Models.Section;
using HelpCenter.Models.User;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models.HelpCenter
{
    public class HelpCenter
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public Icon Icon { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public virtual Product.Product Product { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<Section.Section> Sections{ get; set; }


    }
    public enum Icon
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
