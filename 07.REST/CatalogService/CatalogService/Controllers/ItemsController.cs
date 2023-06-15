using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogService.Models;
using CatalogService.Repositories;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository _repository;

        public ItemsController(ItemsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems(int pageNumber, int pageSize, Guid? categoryId = null)
        {
            var query = _repository.Get(x => !categoryId.HasValue || x.CategoryId == categoryId)
                .OrderBy(on => on.Name)
                .Skip(pageNumber - 1 * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            var item = await _repository.Get(x => x.Id == id).SingleOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Item item)
        {
            _repository.Update(item);

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _repository.Create(item);

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            if (!await _repository.Get(x => x.Id == id).AnyAsync())
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}
