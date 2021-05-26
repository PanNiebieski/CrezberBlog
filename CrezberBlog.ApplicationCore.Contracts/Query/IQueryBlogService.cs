using CrezberBlog.ApplicationCore.Query;
using CrezberBlog.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IQueryBlogService
    {
        void ReloadCache();

        Task<IEnumerable<Post>> GetPosts();

        Task<IEnumerable<Post>> GetPosts(int count, int skip = 0);

        Task<IEnumerable<Post>> GetPostsForBlog(int blogid);

        Task<int> GetHowManyPostsForBlog(int blogid);

        Task<IEnumerable<Post>> GetPostsInBlog(int blogid, int count, int skip = 0, bool checkuser = true);

        Task<IEnumerable<Post>> GetPostInCourseUrlName(string courseUrlName);

        Task<IEnumerable<Post>> GetPostInCourseUrlName(int blogid, string courseUrlName);

        Task<Dictionary<string, List<Post>>> GetPostsGroupByYear();

        Task<Dictionary<string, List<Post>>> GetPostsGroupByYear(int blogid);

        Task<Dictionary<string, List<Post>>> GetPostsGroupByYear(int blogid, int count, int skip = 0);

        Task<Dictionary<string, List<Post>>> GetPostsGroupByYear(int blogid, string category, int count, int skip = 0);

        Task<IEnumerable<Course>> GetCourses();

        Task<IEnumerable<Course>> GetCoursesForBlog(int blogid);

        Task<IEnumerable<CourseGrupedBySubCategory>> GetCourseGrupedBySubCategory();

        Task<IEnumerable<CourseGrupedBySubCategory>> GetCourseGrupedBySubCategory(int blogid);

        Task<IEnumerable<Post>> GetPostsByCategory(string category);

        Task<IEnumerable<Post>> GetPostsByCategory(int blogid, string category);

        Task<IEnumerable<Post>> GetPostsByCategory(int blogid, string category, int count, int skip = 0);

        Task<Post> GetPostBySlug(string slug);

        Task<Post> GetPostInBlogBySlug(int blogid, string slug);

        Task<Post> GetPostById(string id);

        Task<Post> GetPostInBlogById(int blogid, string id);

        Task<IEnumerable<string>> GetCategories();

        Task<IEnumerable<string>> GetCategoriesByBlog(int blogid);


        Task<IEnumerable<CategoryWithNumber>> GetCategoriesWithNumberByBlog();

        Task<IEnumerable<CategoryWithNumber>> GetCategoriesWithNumberByBlog(int blogid);

        Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPosts();

        Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPosts(int blogid);

    }
}
