using System.Collections.Generic;
using System.Linq;
using CategoriesTask.Database;
using CategoriesTask.DTO;

namespace CategoriesTask.Service
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoriesDbContext _context;

        public CategoryService(ICategoriesDbContext context)
        {
            _context = context;
        }

        public Category GetCategoryById(int id)
        {
            var categoryEntity = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (categoryEntity == null)
                return null; //nothing found

            var parentId = categoryEntity.ParentId;

            while (string.IsNullOrEmpty(categoryEntity.Keywords))
            {
                var parent = _context.Categories.Single(x => x.Id == parentId);
                categoryEntity.Keywords = parent.Keywords;
                parentId = parent.ParentId;

                if (!parentId.HasValue)
                    break; //root category, no keywords
            }

            return new Category(categoryEntity);
        }

        public IList<Category> GetCategoriesByLevel(int level)
        {
            //root level = null
            var categories = _context.Categories.Where(x => x.ParentId == null);
            
            for (var i = 0; i < level; i++)
            {
                var parentIds = categories.Select(x => x.Id).ToList();
                categories = _context.Categories.Where(x => parentIds.Contains(x.ParentId.Value));
            }

            return categories.Select(x => new Category(x)).ToList();
        }
    }
}
