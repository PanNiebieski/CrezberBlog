namespace CrezberBlog.Infrastructure.EmailSender
{
    public class EmailOptions
    {
        public string EmailSubjectPrefix { get; set; }

        public bool EnableSsl { get; set; }

        public int SmtpServerPort { get; set; }

        public string SmtpServer { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }

        public string Email { get; set; }
    }
}
