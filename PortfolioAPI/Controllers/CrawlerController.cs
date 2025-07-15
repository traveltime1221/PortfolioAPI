// 對外暴露 Web API 讓前端呼叫 .NET → Python 邏輯
using Microsoft.AspNetCore.Mvc;
// using PortfolioAPI.Services.Services;
using PortfolioAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
// 根據傳進來不同的名稱取得不同的爬蟲資訊
// /api/crawler/earthquake_tw_info: 台灣地震資訊
// /api/crawler/earthquake_info (全球地震資訊)
// /api/crawler/typhoon_info 颱風天停班停課
public class CrawlerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CrawlerController(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }

    [HttpGet("{crawlerName}")]
    public async Task<IActionResult> RunCrawler(string crawlerName)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CrawlerAPI");
            
            // 拼湊網址：http://127.0.0.1:8000/run-spider?type=spider_earthquake_world&date=20250712160522
            string url = $"run-spider?type={crawlerName}";
            
            // 由後端攜帶 date 並推敲 資料夾是否有接近的檔案, 若沒有才去爬, 若有接近的檔案則取得
            DateTime now = DateTime.Now;
            url += $"&date={now.ToString("yyyy-MM-ddTHH:mm:ss")}"; // 2025-07-13T16:05:22
            Console.WriteLine(url);
            
            // var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "tempDownload");
            // 若該資料夾不存在 或 檔案不存在 才去爬蟲
            /*if (!Directory.Exists(tempFolder))
            {
            }*/
            
            var r = await client.GetAsync(url);
            var jsonString = await r.Content.ReadAsStringAsync(); // 先將內容轉字串
            //var json = JsonConvert.DeserializeObject<JObject>(jsonString); // 再將內容轉object
            // 將 content 字串反序列化成真正的 object
            object contentObj = crawlerName switch
            {
                "spider_typhoon_info" => JsonConvert.DeserializeObject<TyphoonInfoRes>(jsonString),
                "spider_earthquake_world" => JsonConvert.DeserializeObject<EarthquakeWorldInfoRes>(jsonString),
                "spider_earthquake_info_tw" => JsonConvert.DeserializeObject<EarthquakeTWInfoRes>(jsonString),
                _ => null
            };
            // var json = JObject.Parse(jsonString);
            // var json = JsonConvert.DeserializeObject<JObject>(jsonString);
                
            return Ok(ApiResponse.Success(contentObj, "操作成功"));

            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}