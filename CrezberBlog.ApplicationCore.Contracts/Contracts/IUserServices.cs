namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IUserServices
    {
        bool ValidateUser(string username, string password);
    }
}
