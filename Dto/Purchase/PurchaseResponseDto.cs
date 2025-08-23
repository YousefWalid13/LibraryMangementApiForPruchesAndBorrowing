namespace LibraryManagementAPI.Dto.Purchases
{
    public class PurchaseResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public DateTime PurchasedAt { get; set; }
    }
}
