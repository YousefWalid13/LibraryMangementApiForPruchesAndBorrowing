using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingRepo _borrowingRepo;

        public BorrowingsController(IBorrowingRepo borrowingRepo)
        {
            _borrowingRepo = borrowingRepo;
        }
        [Authorize(Roles = "Admin , User")]
        [HttpPost("{bookId}/borrow")]
        public async Task<IActionResult> BorrowBook(int bookId, string userId)
        {
            var result = await _borrowingRepo.BorrowAsync(userId, bookId);
            if (!result) return BadRequest("Book not available or already borrowed.");
            return Ok("Book borrowed successfully.");
        }
        [Authorize(Roles = "Admin , User" )]
        [HttpPost("{bookId}/return")]
        public async Task<IActionResult> ReturnBook(int bookId, string userId)
        {
            var result = await _borrowingRepo.ReturnAsync(userId, bookId);
            if (!result) return BadRequest("Book not found in your borrowings.");
            return Ok("Book returned successfully.");
        }

        [HttpGet("my")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> MyBorrowings(string userId)
        {
            var borrowings = await _borrowingRepo.GetUserBorrowingsAsync(userId);
            return Ok(borrowings);
        }
    }

}
