namespace CrezberBlog.ApplicationCore.Contracts.EmailSender
{
    public class EmailStatus
    {
        public EmailStatus()
        {
            Success = true;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
