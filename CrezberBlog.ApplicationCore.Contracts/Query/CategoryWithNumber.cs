using CrezberBlog.Domain;

namespace CrezberBlog.ApplicationCore.Query
{
    public class CategoryWithNumber
    {
        public CategoryNew Name { get; set; }

        public int PostCount { get; set; }
    }
}
