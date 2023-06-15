using CatalogService.Helpers;
using CatalogService.Models;
using System.Linq;
using System.Linq.Expressions;

namespace CatalogService.Repositories
{
    public abstract class Repository<T> where T: class, IIdentifiable
    {
        private readonly CatalogDbContext _context;

        public virtual IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null)
            => GetEntities(_context).Where(predicate ?? (x => true));

        public virtual void Create(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (GetEntities(_context).Any(x => x.Id == item.Id))
                throw new InvalidOperationException($"Item with Id {item.Id} already exists");
            
            _context.Add(item);
            _context.SaveChanges();
        }

        public virtual void Update(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!GetEntities(_context).Any(x => x.Id == item.Id))
                throw new InvalidOperationException($"Item with Id {item.Id} not exists");

            Convert(GetEntities(_context).Single(x => x.Id == item.Id), item);

            _context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            if (!GetEntities(_context).Any(x => x.Id == id))
                throw new InvalidOperationException($"Item with Id {id} not exists");

            _context.Remove(GetEntities(_context).Single(x => x.Id == id));
            _context.SaveChanges();
        }

        public virtual void Update(IEnumerable<T> items)
            => CollectionsMerger.Merge(
                GetEntities(_context),
                items,
                x => x.Id,
                x => x.Id,
                add: x => _context.Add(x),
                update: Convert,
                remove: x => _context.Remove(x)
            );

        protected abstract void Convert(T original, T updated);
        protected abstract Func<CatalogDbContext, IQueryable<T>> GetEntities { get; }
    }
}
