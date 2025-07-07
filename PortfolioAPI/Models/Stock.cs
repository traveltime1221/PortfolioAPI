namespace PortfolioAPI.Models;

public class Stock
{
    public int Id { get; set; }
    public string StockName { get; set; }
    public string StockCode { get; set; }
    public int MarketId { get; set; }
    public Market Market { get; set; }  // 這是 FK 關聯
}