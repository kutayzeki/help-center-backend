using HelpCenter.Models.Section;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models.Item
{
    public class Item
    {

        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        public Type Type{ get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        [Required]
        public Guid SectionId { get; set; }
        public virtual Section.Section Section{ get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
    public enum Type
    {
        None = 100,
        Url = 200,
        Tutorial = 300,
        Rate = 400,
        Announcement = 500,
    }

}
