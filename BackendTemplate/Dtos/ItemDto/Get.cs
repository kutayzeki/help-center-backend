using HelpCenter.Models.Item;

namespace HelpCenter.Dtos.ItemDto
{
    public class GetItems
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string SectionName { get; set; }
        public string? Url { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
