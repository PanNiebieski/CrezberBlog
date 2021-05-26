using CrezberBlog.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface ICategoryService
    {
        IList<CategoryNew> Categories();

        CategoryNew FindCategory(string name);

        Task AddCategoryAsync(CategoryNew color);
    }
}
