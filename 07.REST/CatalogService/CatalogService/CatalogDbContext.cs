using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(item => item.Category)
                .WithMany(category => category.Items)
                .HasForeignKey(item => item.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(category => category.Items)
                .WithOne(item => item.Category)
                .OnDelete(DeleteBehavior.ClientCascade);

            var fruitsCategoryGuid = Guid.NewGuid();
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = fruitsCategoryGuid, Name = "Fruits" });

            var vegetablesCategoryGuid = Guid.NewGuid();
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = vegetablesCategoryGuid, Name = "Vegetables" });

            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Cucumber",
                CategoryId = vegetablesCategoryGuid
            });

            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Potatoe",
                CategoryId = vegetablesCategoryGuid
            });

            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Banana",
                CategoryId = fruitsCategoryGuid
            });

            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = Guid.NewGuid(),
                Name = "Apple",
                CategoryId = fruitsCategoryGuid
            });
        }
    }
}
