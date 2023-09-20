namespace ChinookMusicApp.Models
{
    public class CustomerSpendings
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalSpending { get; set; }
    }
}
