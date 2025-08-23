using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface IBookRepo
    {
        // تجيب كل الكتب (مع pagination + sorting اختيارياً)
        Task<IEnumerable<Book>> GetAllAsync(int pageNumber, int pageSize, string sortBy, string sortOrder);

        // تجيب كتاب واحد بالـ Id
        Task<Book> GetByIdAsync(int id);

        // تضيف كتاب جديد
        Task<Book> AddAsync(Book book);

        // تعدل كتاب
        Task<Book> UpdateAsync(Book book);

        // تحذف كتاب
        Task<bool> DeleteAsync(int id);
    }
}
