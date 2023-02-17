using HelpCenter.Models.Section;


namespace HelpCenter.Dtos.SectionDto
{
    public class GetSections
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Size Size{ get; set; }
        public int Order { get; set; }
        public string HelpCenterName { get; set; }
        public bool IsActive { get; set; }
    }
}
