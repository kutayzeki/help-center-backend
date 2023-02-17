using HelpCenter.Models.Section;

namespace HelpCenter.Dtos.SectionDto
{
    public class SectionUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public Size Size{ get; set; }
        public bool IsActive { get; set; }
    }
}
