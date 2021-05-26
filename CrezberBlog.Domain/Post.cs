using System;
using System.Collections.Generic;

namespace CrezberBlog.Domain
{
    public class Post
    {
        public Post()
        {
            Variables = new List<PostVariables>();
            Categories = new List<CategoryNew>();
            CategoriesString = new List<string>();
        }

        public int ID { get; set; }

        public int ForBlogId { get; set; }

        public string IDChar { get; set; } = DateTime.UtcNow.Ticks.ToString();

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Excerpt { get; set; }

        public string PrivateNotes { get; set; }

        public string ContentAsMetagDescription { get; set; }

        public string Content { get; set; }

        public string ContentShorted { get; set; }

        public string ContentShortedWithOutMore { get; set; }

        public string ContentShortedToRender { get; set; }

        public string ContentToRender { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        public Course Course { get; set; }

        public CourseItem CourseItem { get; set; }

        public IList<string> CategoriesString { get; set; } = new List<string>();

        public IList<CategoryNew> Categories { get; set; } =
            new List<CategoryNew>();

        public IList<PostVariables> Variables { get; set; } = new List<PostVariables>();




    }
}
