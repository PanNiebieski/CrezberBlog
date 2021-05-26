using CrezberBlog.Domain;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface ISaveDeletePostService
    {
        Task SavePost(Post post);

        Task DeletePost(Post post);
    }
}
