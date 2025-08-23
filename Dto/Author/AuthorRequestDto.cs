namespace LibraryManagementAPI.Dto.Author
{
    public class AuthorRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
