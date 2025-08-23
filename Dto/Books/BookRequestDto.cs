namespace LibraryManagementAPI.Dto.Books
{
    public class BookRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public int CopiesAvailable { get; set; }

        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
    }
}
