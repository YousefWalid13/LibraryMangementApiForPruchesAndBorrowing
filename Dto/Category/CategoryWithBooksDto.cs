using LibraryManagementAPI.Dto.Books;

namespace LibraryManagementAPI.Dto.Category
{
    public class CategoryWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // ممكن نعرض قائمة الكتب بشكل مبسط باستخدام BookResponseDto
        public List<BookResponseDto> Books { get; set; } = new();
    }
}
