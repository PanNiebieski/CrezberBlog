using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public class FileSaveDeletePostService : ISaveDeletePostService
    {
        private readonly IWebInfoAboutFolders _webHostEnvironment;


        private BaseCasheContentService _fileCacheService;

        public FileSaveDeletePostService(IWebInfoAboutFolders env,
            BaseCasheContentService fileCacheService)
        {
            _webHostEnvironment = env;
            _fileCacheService = fileCacheService;
        }

        public Task DeletePost(Post post)
        {
            string filePath = _webHostEnvironment.GetWebRootPath() + "/" + GetFilePath(post);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _fileCacheService.RemovePostFromCache(post);

            return Task.CompletedTask;
        }

        public async Task SavePost(Post post)
        {
            string filePath = _webHostEnvironment.GetWebRootPath() + "/" + GetFilePath(post);
            post.LastModified = DateTime.UtcNow;

            XDocument doc = new XDocument(
                            new XElement("post",
                                new XElement("title", post.Title),
                                new XElement("slug", post.Slug),
                                new XElement("pubDate", FormatDateTime(post.PubDate)),
                                new XElement("lastModified", FormatDateTime(post.LastModified)),
                                new XElement("excerpt", post.Excerpt),
                                new XElement("privateNotes", post.PrivateNotes),
                                new XElement("blogId", post.ForBlogId),
                                new XElement("ispublished", post.IsPublished),
                                new XElement("categories", string.Empty),
                                new XElement("variables", string.Empty),
                                new XElement("content", post.Content)
                            ));

            XElement categories = doc.XPathSelectElement("post/categories");
            foreach (string category in post.CategoriesString)
            {
                categories.Add(new XElement("category", category));
            }

            XElement variables = doc.XPathSelectElement("post/variables");
            foreach (var varr in post.Variables)
            {
                var s = new XElement("variable",
                    new XElement("key", varr.Key),
                    new XElement("value", varr.Value));

                variables.Add(s);

            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }

            _fileCacheService.RemovePostFromCache(post);
            _fileCacheService.AddPostFromCache(post);
        }

        private static string FormatDateTime(DateTime dateTime)
        {
            const string UTC = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";

            return dateTime.Kind == DateTimeKind.Utc
                ? dateTime.ToString(UTC)
                : dateTime.ToUniversalTime().ToString(UTC);
        }

        private string GetFilePath(Post post)
        {

            string _folder = "Posts/" + _webHostEnvironment.GetFolderBlogById(post.ForBlogId);
            var result = Path.Combine(_folder, post.IDChar + ".xml");
            return result;
        }

    }
}
