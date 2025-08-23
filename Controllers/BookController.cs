using LibraryManagementAPI.Dto.Books;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo _repo;

        public BookController(IBookRepo repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Admin , User")]
        // ✅ GET: api/book?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetBooks(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "id",
            string sortOrder = "asc")
        {
            var books = await _repo.GetAllAsync(pageNumber, pageSize, sortBy, sortOrder);

            var response = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                Price = b.Price
            });

            return Ok(response);
        }
        [Authorize(Roles = "Admin , User")]
        // ✅ GET: api/book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) return NotFound();

            var response = new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,
                Price = book.Price,
                CategoryName = book.Category?.Name ?? "N/A",
                AuthorName = book.Author?.Name ?? "N/A"
            };

            return Ok(response);
        }

        // ✅ POST: api/book
        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<IActionResult> AddBook([FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublishedDate = dto.PublishedDate,
                Price = dto.Price,
                CopiesAvailable = dto.CopiesAvailable,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId
            };

            var newBook = await _repo.AddAsync(book);

            var response = new BookResponseDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                ISBN = newBook.ISBN,
                Price = newBook.Price
            };

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, response);
        }

        // ✅ PUT: api/book/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDto dto)
        {
            var existingBook = await _repo.GetByIdAsync(id);
            if (existingBook == null) return NotFound();

            existingBook.Title = dto.Title;
            existingBook.ISBN = dto.ISBN;
            existingBook.PublishedDate = dto.PublishedDate;
            existingBook.Price = dto.Price;
            existingBook.CopiesAvailable = dto.CopiesAvailable;
            existingBook.CategoryId = dto.CategoryId;
            existingBook.AuthorId = dto.AuthorId;

            var updatedBook = await _repo.UpdateAsync(existingBook);

            var response = new BookResponseDto
            {
                Id = updatedBook.Id,
                Title = updatedBook.Title,
                ISBN = updatedBook.ISBN,
                Price = updatedBook.Price
            };

            return Ok(response);
        }

        // ✅ DELETE: api/book/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _repo.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
