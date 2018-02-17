using System.Linq;
using Autofac;
using CategoriesTask.Database;
using CategoriesTask.Database.Model;
using CategoriesTask.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CategoriesTask.Test
{
    /// <summary>
    /// This is not real test class, just demo
    /// No negative scenarios, no fake context, no other test features
    /// </summary>
    [TestClass]
    public class IntegrationTestClass
    {
        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            //get autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterModule<CategoryAutofacModule>();
            _container = builder.Build();
            
            using (var scope = _container.BeginLifetimeScope())
            {
                //add test data to the database
                var context = scope.Resolve<ICategoriesDbContext>();
                context.EnsureDeleted();
                context.EnsureCrteated();
                //ParentId for the root categories are changed to NULL instead of -1 in order to prettify the code
                //This change does not impact the logic
                context.Categories.Add(new CategoryEntity{Id = 100, Name = "Business", Keywords = "Money"});
                context.Categories.Add(new CategoryEntity{Id = 200, Name = "Tutoring", Keywords = "Teaching" });
                context.Categories.Add(new CategoryEntity{Id = 101, ParentId = 100, Name = "Accounting", Keywords = "Taxes" });
                context.Categories.Add(new CategoryEntity{Id = 102, ParentId = 100, Name = "Taxation"});
                context.Categories.Add(new CategoryEntity{Id = 201, ParentId = 200, Name = "Computer"});
                context.Categories.Add(new CategoryEntity{Id = 103, ParentId = 101, Name = "Corporate Tax"});
                context.Categories.Add(new CategoryEntity{Id = 202, ParentId = 201, Name = "Operating System"});
                context.Categories.Add(new CategoryEntity{Id = 109, ParentId = 103, Name = "Small business Tax"}); //third level

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetCategoryById_Demo()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var service = scope.Resolve<ICategoryService>();

                var category = service.GetCategoryById(109);
                Assert.AreEqual(103, category.ParentId);
                Assert.AreEqual("Small business Tax", category.Name);
                Assert.AreEqual("Taxes", category.Keywords);
            }
        }

        [TestMethod]
        public void GetCategoriesByLevel_Demo()
        {
            //The very first level (root) is 0
            using (var scope = _container.BeginLifetimeScope())
            {
                var service = scope.Resolve<ICategoryService>();
                //different kind of asserts just for the demo purposes
                var categories = service.GetCategoriesByLevel(0); //100, 200
                Assert.AreEqual(2, categories.Count);
                Assert.IsTrue(categories.Any(x => x.Name.Equals("Business")));
                Assert.IsNotNull(categories.FirstOrDefault(x => x.Name.Equals("Tutoring")));

                categories = service.GetCategoriesByLevel(2); //103, 202
                Assert.AreEqual(2, categories.Count);
                Assert.IsTrue(categories.Any(x => x.Name.Equals("Corporate Tax")));
                Assert.IsNotNull(categories.FirstOrDefault(x => x.Name.Equals("Operating System")));

                categories = service.GetCategoriesByLevel(3); //109
                Assert.AreEqual(1, categories.Count);
                Assert.IsNotNull(categories.Single(x => x.Name.Equals("Small business Tax")));
            }
        }
    }
}
