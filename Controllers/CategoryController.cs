using LibraryManagementAPI.Dto.Category;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IWorkRepo<Category> _categoryRepo;

        public CategoryController(IWorkRepo<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // ✅ GET: api/category?pageNumber=1&pageSize=10&sortBy=Name&sortOrder=asc
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1, int pageSize = 10,
            string sortBy = "id", string sortOrder = "asc")
        {
            var categories = await _categoryRepo.GetAllAsync();


            var result = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return Ok(result);
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = $"Category with Id = {id} not found." });

            var result = new CategoryWithBooksDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Books = category.Books.Select(b => new Dto.Books.BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate
                }).ToList()
            };

            return Ok(result);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var created = await _categoryRepo.AddAsync(category);

            var result = new CategoryResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = $"Category with Id = {id} not found." });

            category.Name = dto.Name;
            category.Description = dto.Description;

            var updated = await _categoryRepo.UpdateAsync(category,id);

            var result = new CategoryResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _categoryRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound(new { Message = $"Author with Id = {id} not found." });

            await _categoryRepo.DeleteAsync(author, id);
            return NoContent();
        }
    }
}
