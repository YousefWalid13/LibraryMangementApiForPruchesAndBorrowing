namespace LibraryManagementAPI.Models
{
    public class Category
    {
        public int Id { get; set; }  // رقم التصنيف
        public string Name { get; set; }
        public string Description { get; set; }

        // علاقة: التصنيف يحتوي على مجموعة كتب
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
