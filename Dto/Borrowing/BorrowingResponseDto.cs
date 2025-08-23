namespace LibraryManagementAPI.Dto.Borrowings
{
    public class BorrowingResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;

        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
