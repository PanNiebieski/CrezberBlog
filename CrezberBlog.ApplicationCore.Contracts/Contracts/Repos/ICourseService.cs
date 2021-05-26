using CrezberBlog.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface ICourseService
    {
        Task<List<Course>> GetCourses(ref List<Post> posts);

        Task SaveCourse(Course course);

        Task DeleteCourse(Course course);
    }
}
