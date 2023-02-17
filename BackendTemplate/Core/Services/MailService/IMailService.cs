using HelpCenter.Models.Mail;

namespace HelpCenter.Core.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);

    }
}
