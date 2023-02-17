
using HelpCenter.Models.HelpCenter;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Dtos.HelpCenterDto
{
    public class HelpCenterUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Icon Icon { get; set; }
        public string Color { get; set; }
        public Position Position { get; set; }
        public bool IsActive { get; set; }
    }
}
