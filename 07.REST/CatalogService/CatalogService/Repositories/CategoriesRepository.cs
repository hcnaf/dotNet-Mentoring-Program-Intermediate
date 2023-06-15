using CatalogService.Helpers;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories
{
    public class CategoriesRepository : Repository<Category>
    {
        protected override Func<CatalogDbContext, IQueryable<Category>> GetEntities => context => context.Categories.Include(x => x.Items);

        protected override void Convert(Category original, Category updated)
        {
            original.Id = updated.Id;
            original.Name = updated.Name;

            CollectionsMerger.Merge(
                original.Items,
                updated.Items,
                x => x.Id,
                x => x.Id,
                add: x => original.Items.Add(x),
                update: (originalItem, updatedItem) => originalItem.Name = updatedItem.Name,
                remove: x => original.Items.Remove(x));
        }
    }
}
