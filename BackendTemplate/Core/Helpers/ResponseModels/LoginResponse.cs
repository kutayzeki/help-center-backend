using BackendTemplate.Models.User;

namespace BackendTemplate.Core.Helpers.ResponseModels
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TokenInfo TokenInfo { get; set; }
        public string Role { get; set; }

    }
}
