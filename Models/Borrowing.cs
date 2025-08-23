namespace LibraryManagementAPI.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }

        // العلاقة مع الكتاب
        public int BookId { get; set; }
        public Book Book { get; set; }

        // العلاقة مع اليوزر (string لأنه IdentityUser.Id)
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
    }
}
