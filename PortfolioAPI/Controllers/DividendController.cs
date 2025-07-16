using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using PortfolioAPI.Common;
// 系統相關內建套件
using System.Text.Json;

namespace PortfolioAPI.Controllers;

public class DividendController : Controller
{

    [HttpGet("/api/Dividends")]
    public async Task<IActionResult> GetDividends()
    {
        var r = await LoadDividendsAsync();
        return Ok(ApiResponse.Success(r));
    }

    [HttpPost("/api/Dividends/Update")]
    public async Task<IActionResult> UpdateDividend([FromBody] DividenAction request)
    {
        var _records = await LoadDividendsAsync();

        if (!Enum.TryParse<DividendActionType>(request.Action, out var action))
        {
            return BadRequest(ApiResponse.Error("未知操作類型"));
        }
        
        switch (action)
        {
            case DividendActionType.Create:
                var _id = _records.Any() ? _records.Max(x => x.id) + 1 : 1;
                // 新增到清單中
                _records.Add(new DividendItem
                {
                    id = _id,
                    StockName = request.StockName,
                    StockCode = request.StockCode,
                    Amount = request.Amount,
                    ReceivedDate = request.ReceivedDate ?? DateTime.Now
                });
                break;
            case DividendActionType.Update:
                // 找到該筆的資料
                var target = _records.FirstOrDefault(x => x.id == request.id);
                if (target == null) return NotFound($"找不到該筆資料");
                
                // 進行做替換動作
                target.StockName = request.StockName;
                target.StockCode = request.StockCode;
                target.Amount = request.Amount;
                target.ReceivedDate = request.ReceivedDate;
                break;
            
            case DividendActionType.Delete:
                var r = _records.RemoveAll(x => x.id == request.id);
                if (r == 0) return NotFound(ApiResponse.Error("找不到該筆資料"));
                break;
            default:
                return BadRequest("不支援的 action");
        }
        await SaveDividendsAsync(_records);
        return Ok(ApiResponse.Success("更新成功"));
    }
    
    private static readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "TempDownload", "Stock", "dividends.json");

    public static async Task SaveDividendsAsync(List<DividendItem> dividends)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true // 格式化好閱讀
        };

        var json = JsonSerializer.Serialize(dividends, options);
        await System.IO.File.WriteAllTextAsync(_jsonPath, json);
    }

    public async Task<List<DividendItem>> LoadDividendsAsync()
    {
        if (!System.IO.File.Exists(_jsonPath))
        {
            return new List<DividendItem>();
        }
        var json = await System.IO.File.ReadAllTextAsync(_jsonPath);
        return JsonSerializer.Deserialize<List<DividendItem>>(json);
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}