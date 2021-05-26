namespace CrezberBlog.ApplicationCore.Contracts.EmailSender
{
    public class MailModel
    {
        public string Name
        {
            get;
            set;
        }

        public string EMail
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public ContactFormMode Mode { get; set; }

        public EmailStatus EmailStatus { get; set; }
    }
}
