using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{

    public interface IWorkRepo<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByNameAsync(string name);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity , int id);
        Task<T?> DeleteAsync(T entity , int id);
    }
}
