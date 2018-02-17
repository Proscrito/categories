using System;
using CategoriesTask.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace CategoriesTask.Database
{
    public interface ICategoriesDbContext : IDisposable
    {
        DbSet<CategoryEntity> Categories { get; set; }

        bool EnsureCrteated();

        bool EnsureDeleted();

        int SaveChanges();
    }
}
