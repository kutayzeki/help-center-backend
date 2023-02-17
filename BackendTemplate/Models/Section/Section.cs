using HelpCenter.Models.HelpCenter;
using HelpCenter.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models.Section
{
    public class Section
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        public Size Size{ get; set; }
        [Required]
        public Guid HelpCenterId { get; set; }
        public virtual HelpCenter.HelpCenter HelpCenter { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<Item.Item> Items { get; set; }


    }
    public enum Size
    {
        Small = 100,
        Medium = 200,
        Large = 300,
    }

}
