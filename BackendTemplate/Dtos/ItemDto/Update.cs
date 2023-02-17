
using HelpCenter.Models.Item;
using System.ComponentModel.DataAnnotations;

namespace Item.Dtos.ItemDto
{
    public class ItemUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string? Url { get; set; }
        public Guid SectionId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
