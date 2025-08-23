using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface IBorrowingRepo
    {
        Task<bool> BorrowAsync(string userId, int bookId);
        Task<bool> ReturnAsync(string userId, int bookId);
        Task<IEnumerable<Borrowing>> GetUserBorrowingsAsync(string userId);
    }
}
