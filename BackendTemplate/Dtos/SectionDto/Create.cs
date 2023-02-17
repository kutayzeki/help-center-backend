using HelpCenter.Models.Section;

namespace HelpCenter.Dtos.SectionDto
{
    public class SectionCreate
    {
        public string Name { get; set; }
        public Size Size{ get; set; }
        public Guid HelpCenterId { get; set; }
        public int Order{ get; set; }
        public bool IsActive { get; set; }

    }
}
