using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface IAuthorRepo
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<Author> AddAsync(Author author);
        Task<Author?> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}
