using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public class FileCacheContentService : BaseCasheContentService
    {
        private readonly IWebInfoAboutFolders _webHostEnvironment;
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IBlogsInfoRepository _blogsInfoRepository;


        public FileCacheContentService(IWebInfoAboutFolders env,
          ICourseService courseService,
            IEnumerable<HtmlPostContentFilter> htmlPostContentFilters,
            ICategoryService categoryService,
            IBlogsInfoRepository blogsInfoRepository
           )
            : base(htmlPostContentFilters)
        {
            _categoryService = categoryService;
            _webHostEnvironment = env;
            _courseService = courseService;
            _blogsInfoRepository = blogsInfoRepository;

            Refresh();
        }

        public override void LoadPosts()
        {

            foreach (var item in _blogsInfoRepository.GetBlogsListOptions())
            {

                string _folder = _webHostEnvironment.GetWebRootPath() + "/" + "Posts/" + item.Name;

                if (!Directory.Exists(_folder))
                    Directory.CreateDirectory(_folder);

                // Can this be done in parallel to speed it up?
                foreach (string file in Directory.EnumerateFiles(_folder, "*.xml", SearchOption.TopDirectoryOnly))
                {
                    XElement doc = XElement.Load(file);

                    Post post = new Post
                    {
                        IDChar = Path.GetFileNameWithoutExtension(file),
                        Title = ReadValue(doc, "title"),
                        Excerpt = ReadValue(doc, "excerpt"),
                        PrivateNotes = ReadValue(doc, "privateNotes"),
                        Content = ReadValue(doc, "content"),
                        Slug = ReadValue(doc, "slug").ToLowerInvariant(),
                        PubDate = DateTime.Parse(ReadValue(doc, "pubDate")),
                        LastModified = DateTime.Parse(ReadValue(doc, "lastModified",
                        DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))),

                        IsPublished = bool.Parse(ReadValue(doc, "ispublished", "true")),
                        ForBlogId = item.Id
                    };

                    LoadVariables(post, doc);

                    if (post.Variables.Count == 0)
                    {
                        post.Variables = GetVariables(post.Content);
                    }

                    post.ContentShortedToRender =
                        HelperString.TrimPostWhitoutMetroTag
                        (post.Content, _webHostEnvironment.AbsoluteUrl(post));

                    var tt = Regex.Replace
                        (post.ContentShortedToRender, "<.*?>", String.Empty);

                    if (tt.Length > 159)
                    {
                        post.ContentAsMetagDescription = tt.Substring(0, 159);
                    }
                    else
                    {
                        post.ContentAsMetagDescription = tt;
                    }

                    //SetOCImageUrl(post, item.Id);

                    LoadCategories(post, doc);
                    //LoadComments(post, doc);
                    CahePosts.Add(post);
                }
            }
            var cas = CahePosts;
            CacheCourse = _courseService.GetCourses(ref cas).Result;

            foreach (var item in CahePosts)
            {
                List<CategoryNew> catnew = new List<CategoryNew>();
                foreach (var c in _categoryService.Categories())
                {
                    if (item.CategoriesString.Contains(c.Name.ToLowerInvariant()))
                    {
                        catnew.Add(c);
                    }
                }
                item.Categories = catnew;
            }

            CahePosts = cas;

            foreach (var post in CahePosts)
            {
                post.ContentToRender = _htmlPostContentFilters.First(k => k.BlogId == post.ForBlogId).
                    RenderString(post);

                post.ContentShorted = HelperString.TrimPost(post.ContentToRender, _webHostEnvironment.AbsoluteUrl(post));

                post.ContentToRender = post.ContentToRender.RemoveWordMore().RemoveWordMetro();

                post.ContentShortedWithOutMore = HelperString.TrimPost(post.ContentToRender, _webHostEnvironment.AbsoluteUrl(post));
            }
        }


        private static string ReadValue(XElement doc, XName name, string defaultValue = "")
        {
            if (doc.Element(name) != null)
                return doc.Element(name)?.Value;

            return defaultValue;
        }

        private static string ReadAttribute(XElement element, XName name, string defaultValue = "")
        {
            if (element.Attribute(name) != null)
                return element.Attribute(name)?.Value;

            return defaultValue;
        }

        private static void LoadVariables(Post post, XElement doc)
        {
            XElement categories = doc.Element("variables");
            if (categories == null)
                return;

            List<PostVariables> list = new List<PostVariables>();

            foreach (var node in categories.Elements("variable"))
            {
                PostVariables PV = new PostVariables();

                PV.Key = node.Element("key").Value;
                PV.Value = node.Element("value").Value;
                //PV.Post = post;

                list.Add(PV);
            }

            post.Variables = list;
        }

        private static void LoadCategories(Post post, XElement doc)
        {
            XElement categories = doc.Element("categories");
            if (categories == null)
                return;

            List<string> list = new List<string>();

            foreach (var node in categories.Elements("category"))
            {
                var s = node.Value.ToLowerInvariant();

                list.Add(s);
            }
            post.CategoriesString = list;
            //post.Categories = list.ConvertToCategories(post.ForBlogId);
        }


        private static List<PostVariables> GetVariables(string content)
        {
            int index = content.IndexOf("data-variables=\"");


            string variables = "";

            if (index != -1)
            {
                index = index + 16;
                int indexlast = content.IndexOf('\"', index);
                variables = content.Substring(index, indexlast - index);
            }

            var tab = variables.Split(',');

            List<PostVariables> dic = new List<PostVariables>(tab.Length);

            foreach (var item in tab)
            {
                var s = item.Split('|');

                if (s.Length == 2)
                {
                    dic.Add(new PostVariables() { Key = s[0], Value = s[1] });
                }

            }

            return dic;
        }

    }
}
