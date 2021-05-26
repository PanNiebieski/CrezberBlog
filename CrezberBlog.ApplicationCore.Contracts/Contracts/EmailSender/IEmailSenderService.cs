using CrezberBlog.ApplicationCore.Contracts.EmailSender;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IEmailSenderService
    {
        EmailStatus SendEmail(string email, string name, string subject, string message);

        EmailStatus SendEmail(MailModel mailModel);

        bool IsValidEmail(string email);

    }
}
