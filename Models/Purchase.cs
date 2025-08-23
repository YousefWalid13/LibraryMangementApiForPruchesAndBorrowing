namespace LibraryManagementAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

        // العلاقة مع الكتاب
        public int BookId { get; set; }
        public Book Book { get; set; }

        // العلاقة مع اليوزر (string لأنه IdentityUser.Id)
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
    }
}
