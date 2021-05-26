using CrezberBlog.Domain;
using System.Collections.Generic;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public abstract class BaseCasheContentService
    {

        public List<Post> CahePosts { get; protected set; }

        public List<Course> CacheCourse { get; protected set; }

        public List<CategoryNew> CacheModelCategory { get; protected set; }

        protected IEnumerable<HtmlPostContentFilter> _htmlPostContentFilters;

        public BaseCasheContentService(IEnumerable<HtmlPostContentFilter> htmlPostContentFilters)
        {
            _htmlPostContentFilters = htmlPostContentFilters;
        }


        public abstract void LoadPosts();

        //public abstract void LoadCategories();

        public void SortCache()
        {
            CahePosts.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
        }

        public void RemovePostFromCache(Post post)
        {
            if (CahePosts.Contains(post))
            {
                CahePosts.Remove(post);
            }
        }

        public void AddPostFromCache(Post post)
        {
            if (!CahePosts.Contains(post))
            {
                CahePosts.Add(post);
                SortCache();
            }
        }

        public void Refresh()
        {
            CacheCourse = new List<Course>();
            CahePosts = new List<Post>();

            LoadPosts();
            SortCache();
        }


    }
}
