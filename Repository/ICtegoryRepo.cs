using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllAsync(int pageNumber, int pageSize, string sortBy, string sortOrder);

        // تجيب كتاب واحد بالـ Id
        Task<Category> GetByIdAsync(int id);

        // تضيف كتاب جديد
        Task<Category> AddAsync(Category Category);

        // تعدل كتاب
        Task<Category> UpdateAsync(Category Category);

        // تحذف كتاب
        Task<bool> DeleteAsync(int id);
    }
}
