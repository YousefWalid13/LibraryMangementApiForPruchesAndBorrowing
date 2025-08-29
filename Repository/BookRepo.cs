using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class BookRepo : IWorkRepo<Book>
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Book?> AddAsync(Book book)
        {
            
            if (book == null)
            {
                return null;
            }
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
            throw new NotImplementedException();
        }

        public async Task<Book?> DeleteAsync(Book book , int id)
        {
            try
            {
               
                 await _context.Books.FindAsync(id);
                if (book == null) { return null; }

                _context.Books.Remove(book);

                await _context.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting the category.", ex);
            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync()

        {
            int pageNumber=0,  pageSize = 0; string? sortBy="",  sortOrder = "";
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


        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Books
                                     .Include(c => c.Category) // التصنيف ومعاه كتبه
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching the category.", ex);
            }

        }

        public async Task<Book?> GetByNameAsync(string name)
        {
            try
            {
                return await _context.Books
                                     .Include(c => c.Category) // التصنيف ومعاه كتبه
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Title == name);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching the category.", ex);
            }
        }

        public async Task<Book?> UpdateAsync(Book book , int id)
        {
            await _context.Books.FindAsync(id);
            if (book == null) { return null; }
            _context.Update(book);
            await _context.SaveChangesAsync();
            return book;

        }

    }
}
