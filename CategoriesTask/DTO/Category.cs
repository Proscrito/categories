using CategoriesTask.Database.Model;

namespace CategoriesTask.DTO
{
    public class Category
    {
        internal Category(CategoryEntity entity)
        {
            ParentId = entity.ParentId ?? -1;
            Name = entity.Name;
            Keywords = entity.Keywords;
        }

        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
    }
}
