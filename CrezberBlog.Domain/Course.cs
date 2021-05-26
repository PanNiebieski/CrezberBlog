using System.Collections.Generic;

namespace CrezberBlog.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NumberText { get; set; }
        public string UrlName { get; set; }
        public bool ShowOnMainPage { get; set; }
        public int ImportantceOrder { get; set; }
        public int ForBlogId { get; set; }
        public bool Visible { get; set; }

        public string SubCategory { get; set; }
        public bool TheBest { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Level { get; set; }
        public bool Completed { get; set; }
        public bool isBig { get; set; }

        public Course()
        {
            Items = new List<CourseItem>();
        }

        public List<CourseItem> Items { get; set; }
    }
}
