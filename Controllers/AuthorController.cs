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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepo _authorRepo;

        public AuthorController(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }
        [Authorize(Roles = "Admin , User")]
        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAll()
        {
            var authors = await _authorRepo.GetAllAsync();

            var result = authors.Select(a => new AuthorResponseDto
            {
                Id = a.Id,
                Name = a.Name
            });

            return Ok(result);
        }

        // GET: api/Author/5
        [Authorize(Roles = "Admin , User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorWithBooksDto>> GetById(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound();

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

        // POST: api/Author
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AuthorResponseDto>> Create(AuthorRequestDto dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBirth = dto.DateOfBirth
            };

            var created = await _authorRepo.AddAsync(author);

            var response = new AuthorResponseDto
            {
                Id = created.Id,
                Name = created.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        // PUT: api/Author/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AuthorRequestDto dto)
        {
            var author = new Author
            {
                Id = id,
                Name = dto.Name,
                Bio = dto.Bio,
                DateOfBirth = dto.DateOfBirth
            };

            var updated = await _authorRepo.UpdateAsync(author);
            if (updated == null)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Author/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _authorRepo.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
