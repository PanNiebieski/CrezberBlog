namespace CrezberBlog.ApplicationCore.Contracts.InfoBlogs
{
    public class BlogBaseOptions
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Theme { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Ico { get; set; }

        public int PostPerPage { get; set; }

        public int OnMainPagePostPerPage { get; set; }


    }
}
