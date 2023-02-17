using HelpCenter.Models.Feedback;
using HelpCenter.Models.HelpCenter;
using HelpCenter.Models.Product;

namespace HelpCenter.Dtos.HelpCenterDto
{
    public class GetHelpCenters
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Icon Icon { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }
    }
}
