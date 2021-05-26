using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.ApplicationCore.Contracts.EmailSender;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CrezberBlog.Infrastructure.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private IStringCipher _stringCipher;
        private EmailOptions _emailOptions;

        public EmailSenderService(
            EmailOptions emailOptions, IStringCipher stringCipher)
        {
            _emailOptions = emailOptions;
            _stringCipher = stringCipher;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                return EmailValidator.Validate(email);
            }
            catch
            {
                return false;
            }
        }

        public EmailStatus SendEmail(string email, string name, string subject, string message)
        {
            EmailStatus status = new EmailStatus();

            string AdminEmail = _emailOptions.Email;
            string EmailSubjectPrefix = _emailOptions.EmailSubjectPrefix;

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(AdminEmail, name);
                    mail.ReplyToList.Add(new MailAddress(email, name));

                    mail.To.Add(AdminEmail);
                    mail.Subject = EmailSubjectPrefix + " - " + subject;

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += System.Web.HttpUtility.HtmlEncode(message).Replace("\n", "<br />") + "<br /><br />";
                    mail.Body += "<hr /><br />";
                    mail.Body += "<h3>" + "Informacje o autorze" + "</h3>";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>" + "Nazwa" + ":</strong> " + System.Web.HttpUtility.HtmlEncode(name) + "<br />";
                    mail.Body += "<strong>" + "Email" + ":</strong> " + System.Web.HttpUtility.HtmlEncode(email) + "<br />";


                    //if (_httpContextAccessor.HttpContext != null)
                    //{
                    //    mail.Body += "<strong>" + " contactIPAddress " + ":</strong> " + GetClientIp() + "<br />";
                    //    mail.Body += "<strong>" + " contactUserAgent " + ":</strong> " + _httpContextAccessor.HttpContext.Request.Headers;
                    //}

                    var error = SendMailMessage(mail);

                    if (error.Length > 0)
                    {
                        status.Success = false;
                        status.ErrorMessage = error;
                        return status;
                    };
                }

                return status;
            }
            catch (Exception ex)
            {
                //L.Log.Error(ex, "problem w public EmailStatus SendEmail(string email, string name, string subject, string message)");


                if (ex.InnerException != null)
                {
                    status.ErrorMessage = ex.InnerException.Message;
                }
                else
                {
                    status.ErrorMessage = ex.Message;
                }

                status.Success = false;

                return status;
            }
        }

        private string SendMailMessage(MailMessage message, string smtpServer = "", string smtpServerPort = "",
        string smtpUserName = "", string smtpPassword = "", string enableSsl = "")
        {
            StringBuilder errorMsg = new StringBuilder();
            bool boolSssl = _emailOptions.EnableSsl;
            int intPort = _emailOptions.SmtpServerPort;
            string SmtpServer = _emailOptions.SmtpServer;
            string SmtpUserName = _emailOptions.SmtpUserName;
            string SmtpPassword = _emailOptions.SmtpPassword;

            SmtpPassword = _stringCipher.DecryptString(SmtpPassword, "E546C8DF278CD5931069B522E695D4F2");

            if (message == null)
                throw new ArgumentNullException("message");

            try
            {
                if (string.IsNullOrEmpty(smtpServer))
                    smtpServer = SmtpServer;

                if (string.IsNullOrEmpty(smtpUserName))
                    smtpUserName = SmtpUserName;

                if (string.IsNullOrEmpty(smtpPassword))
                    smtpPassword = SmtpPassword;

                if (!string.IsNullOrEmpty(smtpServerPort))
                    intPort = int.Parse(smtpServerPort);

                if (!string.IsNullOrEmpty(enableSsl))
                    bool.TryParse(enableSsl, out boolSssl);

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                var smtp = new SmtpClient(smtpServer);
                smtp.UseDefaultCredentials = false;
                // don't send credentials if a server doesn't require it,
                // linux smtp servers don't like that
                if (!string.IsNullOrEmpty(smtpUserName))
                {
                    smtp.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                }

                smtp.Port = intPort;
                smtp.EnableSsl = boolSssl;
                smtp.Send(message);
                //OnEmailSent(message);
            }
            catch (Exception ex)
            {
                //L.Log.Error(ex, "Coś poszło nie tak przy wysyłaniu emaila", message);
                //OnEmailFailed(message);

                errorMsg.Append("Error sending email in SendMailMessage: ");
                Exception current = ex;

                while (current != null)
                {
                    if (errorMsg.Length > 0) { errorMsg.Append(" "); }
                    errorMsg.Append(current.Message);
                    current = current.InnerException;
                }

                //Utils.Log(errorMsg.ToString());
            }
            finally
            {
                // Remove the pointer to the message object so the GC can close the thread.
                message.Dispose();
            }

            return errorMsg.ToString();
        }

        public EmailStatus SendEmail(MailModel mailModel)
        {
            return SendEmail(mailModel.EMail, mailModel.Name, mailModel.Subject, mailModel.Message);
        }
    }
}
