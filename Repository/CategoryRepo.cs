using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // ✅ إضافة تصنيف جديد
        public async Task<Category?> AddAsync(Category category)
        {
            if (category == null) return null;

            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding a new category.", ex);
            }
        }

        // ✅ حذف تصنيف
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) return false;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting the category.", ex);
            }
        }

        // ✅ إحضار كل التصنيفات مع Pagination + Sorting
        public async Task<IEnumerable<Category>> GetAllAsync(int pageNumber, int pageSize, string? sortBy, string? sortOrder)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            try
            {
                IQueryable<Category> query = _context.Categories
                                                     .Include(c => c.Books) // كل تصنيف ومعاه كتبه
                                                     .AsNoTracking();

                // Sorting
                query = sortBy?.ToLower() switch
                {
                    "name" => (sortOrder?.ToLower() == "desc")
                              ? query.OrderByDescending(c => c.Name)
                              : query.OrderBy(c => c.Name),
                    _ => query.OrderBy(c => c.Id) // Default sorting
                };

                // Pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching categories.", ex);
            }
        }

        // ✅ إحضار تصنيف واحد
        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categories
                                     .Include(c => c.Books) // التصنيف ومعاه كتبه
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching the category.", ex);
            }
        }

        // ✅ تحديث تصنيف
        public async Task<Category?> UpdateAsync(Category category)
        {
            if (category == null) return null;

            try
            {
                var existingCategory = await _context.Categories.FindAsync(category.Id);
                if (existingCategory == null) return null;

                // Update fields safely
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                await _context.SaveChangesAsync();
                return existingCategory;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the category.", ex);
            }
        }
    }
}
