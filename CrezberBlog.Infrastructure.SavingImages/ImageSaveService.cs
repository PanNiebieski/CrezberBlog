using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.Domain;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace CrezberBlog.Infrastructure.SavingImages
{
    public class ImageSaveService : IImageSaveService
    {
        private const string FILES = "files";
        private readonly IWebInfoAboutFolders _webInfoAboutFolders;


        public ImageSaveService(IWebInfoAboutFolders webHostEnvironment)
        {
            _webInfoAboutFolders = webHostEnvironment;
        }

        /// <summary>
        /// SaveFile
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException">Ignore.</exception>
        /// <exception cref="FileNotFoundException">Ignore.</exception>
        /// <exception cref="UnauthorizedAccessException">Ignore.</exception>
        /// <exception cref="IOException">Ignore.</exception>
        /// <exception cref="DirectoryNotFoundException">Ignore.</exception>
        /// <exception cref="PathTooLongException">Ignore.</exception>
        public async Task<string> SaveFile(byte[] bytes, string fileName, int blogid, string postname = "", string suffix = null)
        {
            suffix = CleanFromInvalidChars(suffix ?? DateTime.UtcNow.Ticks.ToString());

            string ext = Path.GetExtension(fileName);
            string name = CleanFromInvalidChars(Path.GetFileNameWithoutExtension(fileName));

            string fileNameWithSuffix = $"{name}_{suffix}{ext}";

            string _folder = _webInfoAboutFolders.GetWebRootPath() + "/" + GetFolderBlogPath(blogid);

            var dateFolderFolder = DateTime.Now.Year.ToString();


            string absolute = Path.Combine(_folder, FILES, dateFolderFolder, postname, fileNameWithSuffix);
            string dir = Path.GetDirectoryName(absolute);

            Directory.CreateDirectory(dir);
            using (var writer = new FileStream(absolute, FileMode.CreateNew))
            {
                await writer.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
            }
            return $"{GetFolderBlogPath(blogid)}/{FILES}/{dateFolderFolder}/{postname}/{fileNameWithSuffix}";

            //return $"{GetFolderBlogPath(blogid)}/{FILES}/{fileNameWithSuffix}";
        }

        private string GetFolderBlogPath(int blogid)
        {

            string _folder = "/Posts/" + _webInfoAboutFolders.GetFolderBlogById(blogid);
            return _folder;
        }

        private static string CleanFromInvalidChars(string input)
        {
            // ToDo: what we are doing here if we switch the blog from windows
            // to unix system or vice versa? we should remove all invalid chars for both systems

            var regexSearch = Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()));
            var r = new Regex($"[{regexSearch}]");
            return r.Replace(input, "");
        }

        public async Task SaveFilesToDisk(Post post)
        {
            var imgRegex = new Regex("<img[^>]+ />", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var base64Regex = new Regex("data:[^/]+/(?<ext>[a-z]+);base64,(?<base64>.+)", RegexOptions.IgnoreCase);
            string[] allowedExtensions = new[] {
              ".jpg",
              ".jpeg",
              ".gif",
              ".png",
              ".webp"
            };

            foreach (Match match in imgRegex.Matches(post.Content))
            {
                XmlDocument doc = new XmlDocument();

                var clear = HttpUtility.HtmlDecode(match.Value);

                doc.LoadXml("<root>" + clear + "</root>");

                var img = doc.FirstChild.FirstChild;
                var srcNode = img.Attributes["src"];
                var fileNameNode = img.Attributes["data-filename"];

                // The HTML editor creates base64 DataURIs which we'll have to convert to image files on disk
                if (srcNode != null && fileNameNode != null)
                {
                    string extension = System.IO.Path.GetExtension(fileNameNode.Value);

                    // Only accept image files
                    if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var base64Match = base64Regex.Match(srcNode.Value);
                    if (base64Match.Success)
                    {
                        byte[] bytes = Convert.FromBase64String(base64Match.Groups["base64"].Value);
                        srcNode.Value = await SaveFile(bytes, fileNameNode.Value, post.ForBlogId, post.Slug).ConfigureAwait(false);

                        img.Attributes.Remove(fileNameNode);
                        post.Content = post.Content.Replace(match.Value, img.OuterXml);
                    }
                }
            }
        }
    }
}
