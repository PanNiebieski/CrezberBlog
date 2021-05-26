using CrezberBlog.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IWebinarServices
    {

        Task<List<Webinar>> GetWebinars();

        Task SaveWebinar(Webinar webinar);
    }
}
