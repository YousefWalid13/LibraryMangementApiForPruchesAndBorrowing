using LibraryManagementAPI.Dto.Category;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 يتطلب تسجيل الدخول
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        [Authorize(Roles = "Admin , User")]
        // ✅ Get all categories with pagination + sorting
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll(
            int pageNumber = 1, int pageSize = 10,
            string sortBy = "id", string sortOrder = "asc")
        {
            var categories = await _categoryRepo.GetAllAsync(pageNumber, pageSize, sortBy, sortOrder);

            var result = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return Ok(result);
        }

        // ✅ Get category by Id (with books)
        [Authorize(Roles = "Admin , User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithBooksDto>> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

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

        // ✅ Add new category
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create(CategoryRequestDto dto)
        {
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

        // ✅ Update category
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> Update(int id, CategoryRequestDto dto)
        {
            var category = new Category
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };

            var updated = await _categoryRepo.UpdateAsync(category);
            if (updated == null) return NotFound();

            var result = new CategoryResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        // ✅ Delete category
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryRepo.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
