using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // ✅ إضافة كتاب جديد
        public async Task<Book?> AddAsync(Book book)
        {
            if (book == null) return null;

            try
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return book;
            }
            catch (Exception ex)
            {
                // هنا ممكن تسجل الخطأ في logs
                throw new Exception("Error while adding a new book.", ex);
            }
        }

        // ✅ حذف كتاب
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null) return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Logging
                throw new Exception("Error while deleting the book.", ex);
            }
        }

        // ✅ إحضار كل الكتب مع Pagination + Sorting
        public async Task<IEnumerable<Book>> GetAllAsync(int pageNumber, int pageSize, string? sortBy, string? sortOrder)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            try
            {
                IQueryable<Book> query = _context.Books
                                                 .Include(b => b.Category)
                                                 .Include(b => b.Author)
                                                 .AsNoTracking(); // أأمن وأسرع في القراءة

                // Sorting
                query = sortBy?.ToLower() switch
                {
                    "title" => (sortOrder?.ToLower() == "desc") ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                    "publisheddate" => (sortOrder?.ToLower() == "desc") ? query.OrderByDescending(b => b.PublishedDate) : query.OrderBy(b => b.PublishedDate),
                    "price" => (sortOrder?.ToLower() == "desc") ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price),
                    _ => query.OrderBy(b => b.Id) // Default sorting
                };

                // Pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching books.", ex);
            }
        }
        // ✅ إحضار كتاب واحد
        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Books
                                     .Include(b => b.Category)
                                     .Include(b => b.Author)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching the book.", ex);
            }
        }

       

        // ✅ تحديث كتاب
        public async Task<Book?> UpdateAsync(Book book)
        {
            if (book == null) return null;

            try
            {
                var existingBook = await _context.Books.FindAsync(book.Id);
                if (existingBook == null) return null;

                // Update fields safely
                existingBook.Title = book.Title;
                existingBook.Description = book.Description;
                existingBook.PublishedDate = book.PublishedDate;
                existingBook.ISBN = book.ISBN;
                existingBook.Price = book.Price;
                existingBook.CopiesAvailable = book.CopiesAvailable;
                existingBook.CategoryId = book.CategoryId;
                existingBook.AuthorId = book.AuthorId;

                await _context.SaveChangesAsync();
                return existingBook;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the book.", ex);
            }
        }
    }
}
