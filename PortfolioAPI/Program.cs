using Microsoft.EntityFrameworkCore; // use EF Core
using PortfolioAPI.Data; // use 註冊 DbContext 
using PortfolioAPI.Services;
using PortfolioAPI.Converters;
using PortfolioAPI.Models;
using PortfolioAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();


// Add services to the container.
builder.Services.AddControllersWithViews();

// 注入DI: 註冊 DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<CrawlerService>();

// 注入DI: converter
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()));

// 注入DI: 設定檔相關參數
builder.Services.AddHttpClient();
builder.Services.Configure<CWBSettings>(builder.Configuration.GetSection("CWB"));

// 設定 cors 服務
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// 測試是否有順利連上資料庫
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await using var connection = dbContext.Database.GetDbConnection();
    try
    {
        // 嘗試開啟與資料庫的連線
        await connection.OpenAsync();
        Console.WriteLine("資料庫連線成功！");
        await connection.CloseAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"資料庫連線失敗：{ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// 不要被搜尋到
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
    await next();
});

// CORS 必須在 UseRouting 和 UseAuthorization 之後、MapEndpoints 之前。
app.UseHttpsRedirection();      // 強制 https, 通常在最前面
app.UseStaticFiles();           // 允許處理 wwwroot 內靜態資源（可放前面）
app.UseRouting();               // 建立路由, 很重要
app.UseCors("AllowFrontend");   // CORS 要放在 Routing 之後，才會生效！
app.UseAuthorization();         // 權限檢查
app.MapControllerRoute(         // 路由映射寫在最後
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();