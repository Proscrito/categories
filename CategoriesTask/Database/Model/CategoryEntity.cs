using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CategoriesTask.Database.Model
{
    public class CategoryEntity
    {
        public CategoryEntity()
        {
            Children = new List<CategoryEntity>();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }

        public virtual CategoryEntity Parent { get; set; }
        public virtual IList<CategoryEntity> Children { get; set; }
    }
}