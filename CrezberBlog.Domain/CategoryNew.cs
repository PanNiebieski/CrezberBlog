namespace CrezberBlog.Domain
{
    public class CategoryNew
    {
        public int Id { get; set; }

        public int ForBlogId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Color { get; set; }

        public string UrlName
        {
            get
            {
                return System.Net.WebUtility.UrlEncode(Name.Replace(" ", "-"));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
