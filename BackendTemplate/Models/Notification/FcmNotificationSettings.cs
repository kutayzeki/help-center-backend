namespace BackendTemplate.Models.Notification
{
    public class FcmNotificationSetting
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SenderId { get; set; }
        public string ServerKey { get; set; }
    }
}
