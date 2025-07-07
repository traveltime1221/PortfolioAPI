namespace PortfolioAPI.Models;

public class Holding
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public int Quantity { get; set; }
    public double AveragePrice { get; set; } // 注意資料庫使用的型態, 否則會爆炸, 炸成爆米花
    public double TotalCost { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class AddHolding
{
    public string StockCode { get; set; }
    public int Quantity { get; set; }
    public double AveragePrice { get; set; }
    public double TotalCost { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class RemoveHolding
{
    public string StockCode { get; set; }
}

public class UpdateHolding
{
    public int Id { get; set; }
    public string StockCode { get; set; }
    public int Quantity { get; set; }
    public double TotalCost { get; set; }
}
