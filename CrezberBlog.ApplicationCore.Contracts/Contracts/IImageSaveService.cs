using CrezberBlog.Domain;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IImageSaveService
    {
        Task SaveFilesToDisk(Post post);

        Task<string> SaveFile(byte[] bytes, string fileName, int blogid,
            string postname = "", string suffix = null);
    }
}
