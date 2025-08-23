using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class PurchaseRepo : IPurchaseRepo
    {
        private readonly AppDbContext _context;

        public PurchaseRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Purchase?> PurchaseAsync(string userId, int bookId, decimal price)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return null;

            var purchase = new Purchase
            {
                UserId = userId,
                BookId = bookId,
                Price = price,
                PurchasedAt = DateTime.UtcNow
            };

            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetUserPurchasesAsync(string userId)
        {
            return await _context.Purchases
                .Include(p => p.Book)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}
