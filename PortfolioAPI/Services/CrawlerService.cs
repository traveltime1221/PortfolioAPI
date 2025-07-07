// 封裝呼叫 Python 的邏輯（例如使用 ProcessStartInfo 或 Python.NET）
namespace PortfolioAPI.Services.Services;

using System.Diagnostics; // 可以用 .NET 的 Process 類別來開啟外部程式（像是 Python 腳本）。

public class CrawlerService
{
    // 建立非同步方法
    public async Task<(string output, string error, int exitCode)> RunScriptAsync(string scriptPath)
    {
        var pythonScriptDir = Path.Combine(Directory.GetCurrentDirectory(), "PythonCrawler");
        var psi = new ProcessStartInfo
        {
            FileName =  Path.Combine(pythonScriptDir, "venv/bin/python"), // 這是 Python 執行檔，如果你用 venv，可以換成完整路徑
            Arguments = scriptPath, // 要執行的 Python 腳本
            RedirectStandardOutput = true,  // 抓 stdout（印出來的內容）
            RedirectStandardError = true, // 抓錯誤訊息
            UseShellExecute = false, // 不使用系統 shell，避免視窗跳出
            CreateNoWindow = true, // 不要跳出黑框視窗
            WorkingDirectory = Directory.GetCurrentDirectory() // 執行時的路徑（目前目錄)
        };
        
        // 建立啟動程式
        using var process = new Process { StartInfo = psi };
        process.Start();
        
        // 接收輸出與錯誤訊息
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        process.WaitForExit(); // 等待腳本結束
        return (output, error, process.ExitCode);
    }
}