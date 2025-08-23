namespace LibraryManagementAPI.Models
{
    public class Book
    {
        public int Id { get; set; }  // رقم تسلسلي للكتاب
        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int CopiesAvailable { get; set; }

        // العلاقات
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
