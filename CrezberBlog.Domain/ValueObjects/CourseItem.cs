namespace CrezberBlog.Domain
{
    public class CourseItem
    {

        public int Index { get; set; }

        public string Title { get; set; }

        public bool Visible { get; set; }

        public string Slug { get; set; }

        public int PostId { get; set; }

        public int CourseId { get; set; }

        public Post Post { get; set; }

        public Course Course { get; set; }
    }
}
