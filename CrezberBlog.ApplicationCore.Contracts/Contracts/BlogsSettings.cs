using CrezberBlog.ApplicationCore.Contracts.InfoBlogs;
using System.Collections.Generic;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IBlogsInfoRepository
    {
        List<BlogBaseOptions> GetBlogsListOptions();
    }
}
