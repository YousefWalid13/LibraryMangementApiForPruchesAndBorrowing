// Repositories/IPurchaseRepo.cs
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface IPurchaseRepo<T>
    {
        Task<T?> PurchaseAsync(string userId, int bookId, decimal price);
        Task<IEnumerable<T>> GetUserPurchasesAsync(string userId);
    }


}
