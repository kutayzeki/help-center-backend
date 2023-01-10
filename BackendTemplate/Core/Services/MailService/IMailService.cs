using BackendTemplate.Models.Mail;

namespace BackendTemplate.Core.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);

    }
}
