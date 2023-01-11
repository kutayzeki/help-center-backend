using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BackendTemplate.Models.Notification
{
    public class Notification
    {

        public string DeviceId { get; set; }
        public bool IsAndroidDevice { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
    public class GoogleNotification
    {
        public class DataPayload
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }
        public string Priority { get; set; } = "high";
        public DataPayload Data { get; set; }
        public DataPayload Notification { get; set; }
    }
}
