using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories
{
    public class ItemsRepository : Repository<Item>
    {
        protected override Func<CatalogDbContext, IQueryable<Item>> GetEntities => context => context.Items.Include(x => x.Category);

        protected override void Convert(Item original, Item updated)
        {
            original.Id = updated.Id;
            original.Name = updated.Name;
            original.CategoryId = updated.CategoryId;
        }
    }
}
