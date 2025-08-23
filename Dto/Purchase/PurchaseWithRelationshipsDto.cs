using LibraryManagementAPI.Dto.Books;
using LibraryManagementAPI.Dto.Users;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Dto.Purchases
{
    public class PurchaseWithRelationshipsDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchasedAt { get; set; }

        public UserResponseDto User { get; set; } = new();
        public BookResponseDto Book { get; set; } = new();
    }
}
