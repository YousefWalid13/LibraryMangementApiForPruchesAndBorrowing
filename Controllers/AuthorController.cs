using LibraryManagementAPI.Dto.Author;
using LibraryManagementAPI.Dto.Books;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // كل العمليات محتاجة Auth افتراضيًا
    public class AuthorController : ControllerBase
    {
        private readonly IWorkRepo<Author> _authorRepo;

        public AuthorController(IWorkRepo<Author> authorRepo)
        {
            _authorRepo = authorRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorRepo.GetAllAsync();
            var result = authors.Select(a => new AuthorResponseDto
            {
                Id = a.Id,
                Name = a.Name
            });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound(new { Message = $"Author with Id = {id} not found." });

            var result = new AuthorWithBooksDto
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio,
                DateOfBirth = author.DateOfBirth,
                Books = author.Books.Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorRequestDto model)
        {
            var author = new Author
            {
                Name = model.Name,
                Bio = model.Bio,
                DateOfBirth = model.DateOfBirth,
            };

            var created = await _authorRepo.AddAsync(author);

            var response = new AuthorResponseDto
            {
                Id = created.Id,
                Name = created.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorRequestDto dto)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound(new { Message = $"Author with Id = {id} not found." });

            author.Name = dto.Name;
            author.Bio = dto.Bio;
            author.DateOfBirth = dto.DateOfBirth;

            await _authorRepo.UpdateAsync(author, id);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound(new { Message = $"Author with Id = {id} not found." });

            await _authorRepo.DeleteAsync(author, id);
            return NoContent();
        }
    }

}

