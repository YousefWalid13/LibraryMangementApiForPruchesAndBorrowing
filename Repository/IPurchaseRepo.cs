// Repositories/IPurchaseRepo.cs
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repository
{
    public interface IPurchaseRepo
    {
        Task<Purchase?> PurchaseAsync(string userId, int bookId, decimal price);
        Task<IEnumerable<Purchase>> GetUserPurchasesAsync(string userId);
    }


}
