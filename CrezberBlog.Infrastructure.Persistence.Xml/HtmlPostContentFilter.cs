using CrezberBlog.Domain;
using System.Text.RegularExpressions;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public abstract class HtmlPostContentFilter
    {
        public virtual string RenderString(Post post)
        {
            string result = post.Content;

            var video = "<div class=\"video\"><iframe src=\"https://www.youtube.com/embed/{0}\"  frameborder=\"0\" allowfullscreen></iframe></div>";
            result = Regex.Replace(result, @"\[youtube:(.*?)\]", (Match m) => string.Format(video, m.Groups[1].Value));

            return result;
        }


        public abstract int BlogId { get; }

    }
}
