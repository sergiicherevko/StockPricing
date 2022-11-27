namespace Server.Models
{
    public class UserStock
    {
        public int Id { get; set; }
        public int UserId { get; set; }        
        public int StockId { get; set; }


        public UserStock(int userId, int stockId)
        {
            UserId = userId;
            StockId = stockId;
        }
    }
}
