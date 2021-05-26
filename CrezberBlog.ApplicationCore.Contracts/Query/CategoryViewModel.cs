using CrezberBlog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CrezberBlog.ApplicationCore.Query
{
    public class CategoryViewModel
    {
        public string Title { get; set; }

        public string DisplayName { get; set; }

        public string GetTitleWithFirstBigLetter()
        {
            return FirstCharToUpper(Title);
        }

        public int Number { get; set; }

        public List<Post> Posts;

        public CategoryViewModel()
        {
            Posts = new List<Post>();
        }

        public string FirstCharToUpper(string input) =>
            input switch
            {
                null => "",
                "" => "",
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
    }
}
