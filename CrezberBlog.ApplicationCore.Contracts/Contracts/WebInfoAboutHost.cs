using CrezberBlog.Domain;
using System;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IWebInfoAboutFolders
    {
        string GetWebRootPath();
        string GetFolderBlogById(int id);

        Uri AbsoluteUrl(Post p);
        Uri PermaLinkFoDisqus(Post post);


    }
}
