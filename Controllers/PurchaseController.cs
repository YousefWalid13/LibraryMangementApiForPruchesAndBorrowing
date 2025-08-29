using LibraryManagementAPI.Dto.Purchases;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // لو عايز تخلي الشراء للمستخدمين المسجلين فقط
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepo<Purchase> _purchaseRepo;

        public PurchaseController(IPurchaseRepo<Purchase> purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        // POST: api/purchase
        [HttpPost]
        public async Task<ActionResult<PurchaseResponseDto>> PurchaseBook([FromBody] PurchaseRequestDto request)
        {
            var userId = User?.Identity?.Name ?? string.Empty;
            // أو لو بتستخدم JWT Claims:
            // var userId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var purchase = await _purchaseRepo.PurchaseAsync(userId, request.BookId, request.Price);

            if (purchase == null)
                return BadRequest("Purchase failed. Book or User may not exist.");

            var response = new PurchaseResponseDto
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                UserName = purchase.User?.UserName ?? string.Empty,
                BookId = purchase.BookId,
                BookTitle = purchase.Book?.Title ?? string.Empty,
                Price = purchase.Price,
                PurchasedAt = purchase.PurchasedAt
            };

            return Ok(response);
        }

        // GET: api/purchase/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PurchaseResponseDto>>> GetUserPurchases(string userId)
        {
            var purchases = await _purchaseRepo.GetUserPurchasesAsync(userId);

            var response = purchases.Select(p => new PurchaseResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = p.User?.UserName ?? string.Empty,
                BookId = p.BookId,
                BookTitle = p.Book?.Title ?? string.Empty,
                Price = p.Price,
                PurchasedAt = p.PurchasedAt
            });

            return Ok(response);
        }

        // GET: api/purchase/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseResponseDto>> GetPurchaseById(int id)
        {
            var purchases = await _purchaseRepo.GetUserPurchasesAsync(User?.Identity?.Name ?? string.Empty);
            var purchase = purchases.FirstOrDefault(p => p.Id == id);

            if (purchase == null)
                return NotFound("Purchase not found.");

            var response = new PurchaseResponseDto
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                UserName = purchase.User?.UserName ?? string.Empty,
                BookId = purchase.BookId,
                BookTitle = purchase.Book?.Title ?? string.Empty,
                Price = purchase.Price,
                PurchasedAt = purchase.PurchasedAt
            };

            return Ok(response);
        }
    }
}
