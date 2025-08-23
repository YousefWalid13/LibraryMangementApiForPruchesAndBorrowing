using LibraryManagementAPI.Dto.Books;

namespace LibraryManagementAPI.Dto.Author
{
    public class AuthorWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // كتب المؤلف
        public List<BookResponseDto> Books { get; set; } = new();
    }
}
