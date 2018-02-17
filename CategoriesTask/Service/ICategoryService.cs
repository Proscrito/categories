using System.Collections.Generic;
using CategoriesTask.DTO;

namespace CategoriesTask.Service
{
    public interface ICategoryService
    {
        Category GetCategoryById(int id);
        IList<Category> GetCategoriesByLevel(int level);
    }
}
