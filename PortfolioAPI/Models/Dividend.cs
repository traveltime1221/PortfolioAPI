namespace PortfolioAPI.Models;

public enum DividendActionType
{
    Create,
    Update,
    Delete,
}

public class DividenAction
{
    public int id { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string StockName { get; set; }
    public string StockCode { get; set; }
    public decimal Amount { get; set; }
    public string Action { get; set; } // 'create' | 'update' | 'delete'
}

public class DividendItem
{
    public int id { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string StockName { get; set; }
    public string StockCode { get; set; }
    public decimal Amount { get; set; }
}