using CrezberBlog.Domain;
using System.Collections.Generic;

namespace CrezberBlog.ApplicationCore.Query
{
    public class CourseGrupedBySubCategory
    {
        public IEnumerable<Course> Course { get; set; }

        public string SubCategory { get; set; }
    }
}
