// 對外暴露 Web API 讓前端呼叫 .NET → Python 邏輯
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Services.Services;
using PortfolioAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
// 根據傳進來不同的名稱取得不同的爬蟲資訊
// api/crawler/earthquake_tw_info: 台灣地震資訊
// api/crawler/earthquake_info (全球地震資訊)
// api/crawler/typhoon_info 颱風天停班停課
public class CrawlerController : Controller
{
    private readonly CrawlerService _service = new CrawlerService();

    [HttpGet("{crawlerName}")]
    public async Task<IActionResult> RunCrawler(string crawlerName)
    {
        string scriptFile = crawlerName switch
        {
            "earthquake_tw_info" => "PythonCrawler/spiders/spider_earthquake_info_tw.py",
            "earthquake_info" => "PythonCrawler/spiders/spider_earthquake_world.py",
            "typhoon_info" => "PythonCrawler/spiders/spider_typhoon_info.py",
            _ => null
        };
        
        if (scriptFile == null) return BadRequest(new { error = "爬蟲檔名名稱錯誤." });
        var (output, error, exitCode) = await _service.RunScriptAsync(scriptFile);
        
        if (exitCode == 0)
        {
            Type targetType = crawlerName switch
            {
                "earthquake_tw_info" => typeof(EarthquakeTWInfo),
                "earthquake_info" => typeof(EarthquakeWorldInfo),
                "typhoon_info" => typeof(TyphoonInfo),
                _ => null
            };
            if (targetType == null) return BadRequest(ApiResponse.Error("", "API名稱解析錯誤" ));
            
            //var listType = typeof(List<>).MakeGenericType(targetType); // 動態 type
            var json = JsonConvert.DeserializeObject<JObject>(output); // 反序列化：原 string 轉 json
            //var content = json["content"].ToObject(listType);
            
            object contentObj = crawlerName switch
            {
                "typhoon_info" => json["content"].ToObject<TyphoonInfo>(),
                "earthquake_info" => json["content"].ToObject<List<EarthquakeWorldInfo>>(),
                "earthquake_tw_info" => json["content"].ToObject<List<EarthquakeTWInfo>>(),
                _ => null
            };
            
            return Ok(ApiResponse.Success(contentObj, "操作成功"));
        }
        else
        {
            // return StatusCode(500, new { success = false, error });
            return BadRequest(ApiResponse.Error("", "解析錯誤" ));
        }
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}