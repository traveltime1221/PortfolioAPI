using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Services;

// 資料庫邏輯集中處理
public class StockService
{
    // private readonly DbContext context;
    private readonly ApplicationDbContext _context; // 聯想：js 裡面的 require

    public StockService(ApplicationDbContext context)
    {
        _context = context; // 聯想 : 深層拷貝
    }

    // 取得所有股票列表
    public async Task<List<Stock>> GetStocksAsync()
    {
        return await _context.Stocks.ToListAsync(); // 完全讀取資料後會自動釋放
    }

    // 取得所有持有股票列表
    public async Task<List<Holding>> GetHoldingStocks()
    {
        return await _context.Holdings.ToListAsync();
    }

    // create
    public async Task<Holding> AddHoldingStock(AddHolding holding)
    {
        // 先比對 從 Stocks 表單中篩選出是否有該筆資料
        var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.StockCode == holding.StockCode);
        if (stock == null) throw new Exception("找不到對應股票");
        
        // 不確定是否有方法可以省略這段 (可以聯想成組成一個新物件)
        var newHolding = new Holding
        {
            StockId = stock.Id,
            Quantity = holding.Quantity,
            AveragePrice = holding.AveragePrice,
            TotalCost = holding.TotalCost,
            PurchaseDate = holding.PurchaseDate
        };
        
        // 有的話組合成 holdings 表需要的資料欄位 並存入
        _context.Holdings.Add(newHolding);
        await _context.SaveChangesAsync();
        return newHolding;
    }
    
    // 刪除持有股票
    public async Task<Holding> remvoeHolding(string stockCode)
    {
        // 先比對是否有該股票
        var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.StockCode == stockCode);
        if (stock == null) throw new Exception("找不到對應股票");
        
        // 比對是否持有該股票, 需使用 id 做比對
        var holdStock = await _context.Holdings.FirstOrDefaultAsync(s => s.StockId == stock.Id);
        if (holdStock == null) throw new Exception("尚未持有該股票");
        
        // 刪除該股票紀錄
        _context.Holdings.Remove(holdStock);
        await _context.SaveChangesAsync();
        
        return holdStock;
    }

    // 編輯持有股票
    public async Task<Holding> UpdateHoldingStock(UpdateHolding updateHolding)
    {
        // 比對股票列表
        // var stock = _context.Stocks.FirstOrDefault(s => s.StockCode == updateHolding.StockCode);
        // if (stock == null) throw new Exception("股票列表無此股票");
        
        // 編輯該股票 比對：stockId 和 holding.Id 同一筆
        // FirstOrDefaultAsync 是非同步
        // var updateStock = await _context.Holdings.FirstOrDefaultAsync(s => s.Id == updateHolding.Id);
        
        // FindASync 用主健查找
        var updateStock = await _context.Holdings.FindAsync(updateHolding.Id);
        updateStock.Quantity = updateHolding.Quantity;
        updateStock.TotalCost = updateHolding.TotalCost;
        
        await _context.SaveChangesAsync();
        return updateStock;
        
    }






// remove

    // update

    
}