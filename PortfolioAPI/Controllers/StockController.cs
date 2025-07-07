using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Services;
using PortfolioAPI.Common;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

public class StockController : Controller
{
    private readonly StockService _service;

    public StockController(StockService service) => _service = service;

    [HttpGet("/api/Stock")]
    public async Task<IActionResult> GetStocks()
    {
        var r = await _service.GetStocksAsync();
        Console.WriteLine(r.Count);
        return Ok(ApiResponse.Success(r, "操作成功"));
    }
    
    [HttpGet("/api/GetHoldingStocks")]
    public async Task<IActionResult> GetHoldingStocks()
    {
        var r = await _service.GetHoldingStocks();
        return Ok(ApiResponse.Success(r, "操作成功"));
    }

    [HttpPost("/api/Stock/AddHoldingStock")]
    public async Task<IActionResult> AddHoldingStock([FromBody] AddHolding addHolding)
    {
        try
        {
            if (addHolding == null) return BadRequest(ApiResponse.Error("", "參數攜帶錯誤" ));
            
            // 這邊或許要防呆
            var r = await _service.AddHoldingStock(addHolding);
            return Ok(ApiResponse.Success(r, "新增成功"));
        }
        catch (Exception e)
        {
            // 應該要寫出防止出錯的部分
            return BadRequest(ApiResponse.Error("", "伺服器內部錯誤" ));
        }
    }

    [HttpDelete("/api/Stock/RemoveHoldingStock")]
    public async Task<IActionResult> RemoveHoldingStock([FromBody] RemoveHolding removeHolding)
    {
        try
        {
            // 需要確認我要刪除哪一筆
            if (removeHolding == null) return BadRequest(ApiResponse.Error("", "參數攜帶錯誤"));
            var r = await _service.remvoeHolding(removeHolding.StockCode);
            return Ok(ApiResponse.Success(r, "刪除成功"));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Error("", "伺服器內部錯誤"));
        }
    }

    [HttpPut("/api/Stock/UpdateHoldingStock")]
    public async Task<IActionResult> UpdateHoldingStock([FromBody] UpdateHolding updateHolding)
    {
        try
        {
            if (updateHolding == null) return BadRequest(ApiResponse.Error("", "參數攜帶錯誤"));
            var r = await _service.UpdateHoldingStock(updateHolding);
            return Ok(ApiResponse.Success(r));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse.Error("", "伺服器內部錯誤"));
        }
    }

// GET
    public IActionResult Index()
    {
        return View();
    }
}
