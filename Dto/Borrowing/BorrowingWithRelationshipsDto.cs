using LibraryManagementAPI.Dto.Books;
using LibraryManagementAPI.Dto.Users;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Dto.Borrowings
{
    public class BorrowingWithRelationshipsDto
    {
        public int Id { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public UserResponseDto User { get; set; } = new();
        public BookResponseDto Book { get; set; } = new();
    }
}
