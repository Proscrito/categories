using CategoriesTask.Database.Model;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CategoriesTask.Database
{
    internal class CategoriesDbContext : DbContext, ICategoriesDbContext
    {
        private const string _databaseFile = "categories.db";

        public Microsoft.EntityFrameworkCore.DbSet<CategoryEntity> Categories { get; set; }
        public bool EnsureCrteated()
        {
            return Database.EnsureCreated();
        }

        public bool EnsureDeleted()
        {
            return Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"Data Source={_databaseFile}";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
