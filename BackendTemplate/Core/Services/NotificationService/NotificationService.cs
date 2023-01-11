using BackendTemplate.Models.Notification;
using Microsoft.Extensions.Options;
using static BackendTemplate.Models.Notification.GoogleNotification;
using System.Net.Http.Headers;
using System.Runtime;
using CorePush.Google;
using BackendTemplate.Core.Helpers.ResponseModels;
using CorePush.Apple;

namespace BackendTemplate.Core.Services.NotificationService
{
    public interface INotificationService
    {
        Task<ApiResponseViewModel> SendNotification(Notification notification);
    }

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<ApiResponseViewModel> SendNotification(Notification notificationModel)
        {
            ApiResponseViewModel response = new();
            try
            {
                if (notificationModel.IsAndroidDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new()
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    };
                    HttpClient httpClient = new();

                    string authorizationKey = string.Format("key={0}", settings.ServerKey);
                    string deviceToken = notificationModel.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new()
                    {
                        Title = notificationModel.Title,
                        Body = notificationModel.Body
                    };

                    GoogleNotification notification = new();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}
