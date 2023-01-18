using FeedbackHub.Models.Mail;

namespace FeedbackHub.Core.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);

    }
}
