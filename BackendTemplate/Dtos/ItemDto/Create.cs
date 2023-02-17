using HelpCenter.Models.Item;

namespace HelpCenter.Dtos.ItemDto
{
    public class HelpCenterCreate
    {
        public string Name { get; set; }
        public ItemType Type{ get; set; }
        public Guid SectionId { get; set; }
        public string? Url { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

    }
}
