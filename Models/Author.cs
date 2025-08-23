
namespace LibraryManagementAPI.Models
{
    public class Author
    {
        public int Id { get; set; }  // رقم المؤلف
        public string Name { get; set; }
      

        // علاقة: المؤلف ممكن يكون له كتب كتيرة
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public string? Bio { get; internal set; }
        public DateTime? DateOfBirth { get; internal set; }
    }
}
