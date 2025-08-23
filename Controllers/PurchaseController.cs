using LibraryManagementAPI.Dto.Purchases;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // لازم يكون المستخدم مسجل دخول
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepo _purchaseRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseController(IPurchaseRepo purchaseRepo, UserManager<ApplicationUser> userManager)
        {
            _purchaseRepo = purchaseRepo;
            _userManager = userManager;
        }

        // POST: api/Purchase
        [HttpPost]
        public async Task<IActionResult> PurchaseBook([FromBody] PurchaseRequestDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var purchase = await _purchaseRepo.PurchaseAsync(user.Id, request.BookId, request.Price);
            if (purchase == null) return NotFound("Book not found");

            var response = new PurchaseResponseDto
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                UserName = user.UserName ?? "",
                BookId = purchase.BookId,
                BookTitle = purchase.Book?.Title ?? "",
                Price = purchase.Price,
                PurchasedAt = purchase.PurchasedAt
            };

            return Ok(response);
        }

        // GET: api/Purchase/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyPurchases()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var purchases = await _purchaseRepo.GetUserPurchasesAsync(user.Id);

            var response = purchases.Select(p => new PurchaseWithRelationshipsDto
            {
                Id = p.Id,
                Price = p.Price,
                PurchasedAt = p.PurchasedAt,
                User = new Dto.Users.UserResponseDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    
                },
                Book = new Dto.Books.BookResponseDto
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                   
                    ISBN = p.Book.ISBN,
                    PublishedDate = p.Book.PublishedDate,
                  
                }
            });

            return Ok(response);
        }
    }
}
