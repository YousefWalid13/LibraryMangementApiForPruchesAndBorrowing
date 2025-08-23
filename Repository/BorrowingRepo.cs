using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class BorrowingRepo : IBorrowingRepo
    {
        private readonly AppDbContext _context;

        public BorrowingRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BorrowAsync(string userId, int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            bool alreadyBorrowed = await _context.Borrowings
                .AnyAsync(b => b.BookId == bookId && b.ReturnedAt == null);

            if (alreadyBorrowed) return false;

            var borrowing = new Borrowing
            {
                UserId = userId,
                BookId = bookId,
                BorrowedAt = DateTime.UtcNow
            };

            await _context.Borrowings.AddAsync(borrowing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnAsync(string userId, int bookId)
        {
            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId && b.ReturnedAt == null);

            if (borrowing == null) return false;

            borrowing.ReturnedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Borrowing>> GetUserBorrowingsAsync(string userId)
        {
            return await _context.Borrowings
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
