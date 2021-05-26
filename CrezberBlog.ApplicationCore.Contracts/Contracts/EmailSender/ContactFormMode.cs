namespace CrezberBlog.ApplicationCore.Contracts.EmailSender
{
    public enum ContactFormMode
    {
        Email,
        NotFulledAllFields,
        SendedOK,
        SendedErrorWrongEmail,
        SendedErrorAdminProblem,
        SendedErrorWrongCaptcha,
    }
}
