using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using System.Text.Json;
using PortfolioAPI.Common;

namespace PortfolioAPI.Controllers;

public class PortfolioController : Controller
{

    private static readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "TempDownload", "Portfolio", "Portfolio.json");
    
    // api
    [HttpGet("/api/Portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        var r = await LoadPortfolioAsync();
        return Ok(ApiResponse.Success(r));
    }

    [HttpPost("/api/Portfolio/Update")]
    public async Task<IActionResult> UpdatePortfolio([FromBody] PortfolioAction request)
    {
        var _records = await LoadPortfolioAsync();
        if (!Enum.TryParse<PortfolioActionType>(request.Action, out var action))
        {
            return BadRequest(ApiResponse.Error("未知操作類型"));
        }

        switch (action)
        {
            case PortfolioActionType.Create:
                var _id = _records.Any() ? _records.Max(a => a.id) + 1 : 1;
                _records.Add(new PortfolioItem
                {
                    id = _id,
                    title = request.title,
                    description = request.description,
                    image_url = request.image_url,
                    tags = request.tags,
                    created_at = DateTime.Now,
                    document_path = request.document_path,
                    external_link = request.external_link,
                });
                break;
            case PortfolioActionType.Update:
                var target = _records.Find(a => a.id == request.id);
                if (target == null) return NotFound(ApiResponse.Error("找不到該筆資料"));
                
                target.title = request.title;
                target.description = request.description;
                target.image_url = request.image_url;
                target.tags = request.tags;
                target.updated_at = DateTime.Now;
                target.external_link = request.external_link;
                target.document_path = request.document_path;
                break;
                
            case PortfolioActionType.Delete:
                var r = _records.RemoveAll(x => x.id == request.id);
                if (r == 0) return NotFound(ApiResponse.Error("找不到該筆資料"));
                break;
        }
        
        await SavePortfolioAsync(_records);
        return Ok(ApiResponse.Success(_records));
    }




    // 儲存資料到該文件, 還有圖片要製作
    public static async Task SavePortfolioAsync(List<PortfolioItem> portfolioItems)
    {
        // 縮排設定, 用於好閱讀
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        // 反序列轉為字串並寫入
        var json = JsonSerializer.Serialize(portfolioItems, options);
        await System.IO.File.WriteAllTextAsync(_jsonPath, json);
    }
    
    // 讀取文件
    public async Task<List<PortfolioItem>> LoadPortfolioAsync()
    {
        // 先判斷該文件是否存在
        if (!System.IO.File.Exists(_jsonPath)) return new List<PortfolioItem>();
        
        // 讀取該分資料解析成字串, 回傳解析成物件
        var json = await System.IO.File.ReadAllTextAsync(_jsonPath);
        return JsonSerializer.Deserialize<List<PortfolioItem>>(json);
    }


    // GET
    public IActionResult Index()
    {
        return View();
    }
}