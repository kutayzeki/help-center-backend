using HelpCenter.Models.HelpCenter;

namespace HelpCenter.Dtos.HelpCenterDto
{
    public class HelpCenterCreate
    {
        public string Name { get; set; }
        public Icon Icon { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        public Guid ProductId { get; set; }
        public bool IsActive { get; set; }

    }
}
