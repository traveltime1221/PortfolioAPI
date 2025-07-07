using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using Microsoft.Extensions.Options;

namespace PortfolioAPI.Controllers;

// ControllerBase 是給純 API 用的，如果你不需要回傳 View，建議用它。
// Controller 是 MVC 用的，若你也有 Index() 想回傳 Razor View，再保留 Controller。
[ApiController]
[Route("/api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory; // 呼叫API使用 （想成axios fetch)
    private readonly CWBSettings _cwbSettings;

    public WeatherController(IHttpClientFactory httpClientFactory, IOptions<CWBSettings> cwbSettings)
    {
        _httpClientFactory = httpClientFactory;
        _cwbSettings = cwbSettings.Value;
    }
    
    [HttpGet("Weekly")]
    public async Task<IActionResult> Get([FromQuery] string city = "台中市")
    {
        city = System.Web.HttpUtility.HtmlEncode(city);
        var url = $"{_cwbSettings.BaseUrl}F-D0047-091?Authorization={_cwbSettings.ApiKey}&locationName={city}&sort=time";
        // Console.WriteLine(url);
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}