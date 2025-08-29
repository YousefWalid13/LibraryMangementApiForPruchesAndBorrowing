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
        private readonly IWorkRepo<Book> _bookRepo;

        public BookController(IWorkRepo<Book> bookRepo)
        {
            _bookRepo = bookRepo;
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepo.GetAllAsync();

            var result = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                Price = b.Price,
                PublishedDate = b.PublishedDate
            });

            return Ok(result);
        }

        
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { Message = $"Book with Id = {id} not found." });

            var result = new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,
                Price = book.Price,
                CategoryName = book.Category?.Name ?? "N/A",
                AuthorName = book.Author?.Name ?? "N/A"
            };

            return Ok(result);
        }

      
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequestDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                PublishedDate = model.PublishedDate,
                Price = model.Price,
                CopiesAvailable = model.CopiesAvailable,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId
            };

            var created = await _bookRepo.AddAsync(book);

            var response = new BookResponseDto
            {
                Id = created.Id,
                Title = created.Title,
                ISBN = created.ISBN,
                Price = created.Price,
                PublishedDate = created.PublishedDate
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { Message = $"Book with Id = {id} not found." });

            book.Title = dto.Title;
            book.ISBN = dto.ISBN;
            book.PublishedDate = dto.PublishedDate;
            book.Price = dto.Price;
            book.CopiesAvailable = dto.CopiesAvailable;
            book.CategoryId = dto.CategoryId;
            book.AuthorId = dto.AuthorId;

            var updated = await _bookRepo.UpdateAsync(book,id);

            var response = new BookResponseDto
            {
                Id = updated.Id,
                Title = updated.Title,
                ISBN = updated.ISBN,
                Price = updated.Price,
                PublishedDate = updated.PublishedDate
            };

            return Ok(response);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _bookRepo.GetByIdAsync(id);
            if (author == null)
                return NotFound(new { Message = $"Author with Id = {id} not found." });

            await _bookRepo.DeleteAsync(author, id);
            return NoContent();
        }
    }
}
