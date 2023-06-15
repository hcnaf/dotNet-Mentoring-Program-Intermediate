using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogService.Models;
using CatalogService.Repositories;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesRepository _repository;

        public CategoriesController(CategoriesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _repository.Get().ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _repository.Get(x => x.Id == id).SingleOrDefaultAsync();

            if (category is null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Category category)
        {
            _repository.Update(category);

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public ActionResult<Category> PostCategory(Category category)
        {
            _repository.Create(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (!_repository.Get(x => x.Id == id).Any())
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}
