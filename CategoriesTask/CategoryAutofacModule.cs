using Autofac;
using CategoriesTask.Database;
using CategoriesTask.Service;

namespace CategoriesTask
{
    public class CategoryAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //use autofac to handle Dispose()
            builder.Register(c => new CategoriesDbContext()).As<ICategoriesDbContext>();
            builder.Register(c => new CategoryService(c.Resolve<ICategoriesDbContext>())).As<ICategoryService>();
        }
    }
}
