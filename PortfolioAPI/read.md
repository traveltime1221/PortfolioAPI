# 紀錄



## 後端 CRUD 撰寫

- 建立Model /Models/Coupon.cs

## 資料庫相關設定
- appsetting.json 設定資料庫連線（繞過SSL)
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Stock;MultipleActiveResultSets=true;User Id=sa;Password=RDTEST@104437;TrustServerCertificate=True;"
}
```

- 設定 EF Core, 安裝套件

- 當你使用 .NET 8 Native AOT 來發布時，EF Core 無法在執行時動態建構模型 (Model Building)，因為 Native AOT 不支援反射。EF Core 預設是使用 動態建構 (OnModelCreating 方法) 來設定資料庫模型，但在 Native AOT 模式下這樣做會導致錯誤。
```
  dotnet add package Microsoft.EntityFrameworkCore.AOT
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer
  
  - 使用 LINQ 查詢資料庫
  - 將資料寫入資料庫（插入、更新）
  - 執行基本的資料庫操作
```

- 新增資料夾 Data/ApplicationDbContext.cs 並作相關設定
```
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Models;

namespace PortfolioAPI.Data;
public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stock { get; set; }
}
```

- program.cs 注入
```
using Microsoft.EntityFrameworkCore; // use EF Core
using PortfolioAPI.Data; // use 註冊 DbContext 

...(略)

// 注入DI: 註冊 DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## coupon API

- 後端 CRUD 撰寫
  =========================
取得所有持有股票
get: /api/getHoldingStocks

成功：
{
"status": "success",
"message": "操作成功",
"content": [
  {
    "id": 2,
    "stockName": "Apple Inc.",
    "stockCode": "AAPL",
    "marketId": 1,
    "market": null
  }
],
"errorDetail": null
}

失敗：
{
"status": "success",
"message": "操作成功",
"content": "操作失敗"
"errorDetail": null
}

==================================================
幣別列表
get: /api/Currencies

成功：
{  
"status": "success",  
"message": "操作成功",  
"content": [
  {
    no: "1"
    name: "TWD"
  }
],
"errorDetail": null
}

失敗：
{
}

==================================================
股票類別列表

成功
失敗

==================================================
取得所有股票列表 （下拉選單使用）
get: /api/Stocks

成功：
{
"status": "success",
"message": "操作成功",
"content": [
{
"id": 2,
"stockName": "Apple Inc.",
"stockCode": "AAPL",
"marketId": 1,
"market": null
}
],
"errorDetail": null
}

失敗：
{
"status": "success",
"message": "操作成功",
"content": "操作失敗"
"errorDetail": null
}



==================================================
新增持有股票
post: /api/Stock/AddHoldingStock
{
  StockCode: '', // 股票列表取得
  Quantity: '', // 數量
  AveragePrice: '', // 平均價格
  PurchaseDate: "", // 購入日期
  TotalCost: "" // 買入價格
}
範例：
{ 
  StockCode: "2330", 
  Quantity: 10, 
  AveragePrice: 500, 
  PurchaseDate: "2024-01-01 20:00:00",
  TotalCoust: 200,
}

成功：
{  
"status": "success",  
"message": "操作成功",  
"content": "操作成功",
"errorDetail": null
}

失敗：
{  
"status": "error",  
"message": "伺服器內部錯誤",  
"content": null,  
"errorDetail": "Object reference not set to an instance of an object."
}


// 增量操作
假設新增後有同筆資料 則開始做成本及平均成本計算



==================================================
編輯股票
update: /api/Stock/UpdateStock
{
Id: '', // 唯一編號
StockCode: '', // 股票列表取得
Quantity: '', // 數量
TotalCost: "" // 買入價格


// 後端需要尋找比對股票並計算成本

成功：
{  
"status": "success",  
"message": "操作成功",  
"content": "操作成功",
"errorDetail": null
}

失敗：
{  
"status": "error",  
"message": "伺服器內部錯誤",  
"content": null,  
"errorDetail": "Object reference not set to an instance of an object."
}


==================================================
刪除股票
delete: /api/Stock/DeleteStock

{
StockCode: '',
}

成功：
{  
"status": "success",  
"message": "操作成功",  
"content": "操作成功",
"errorDetail": null
}

失敗：
{  
"status": "error",  
"message": "伺服器內部錯誤",  
"content": null,  
"errorDetail": "Object reference not set to an instance of an object."
}

## 20250615_python 設定
### 資料夾結構規劃
MyProject/
│
├── Controllers/
│   └── CrawlerController.cs         ← 提供 HTTP API 控制爬蟲執行
│
├── Services/
│   └── CrawlerService.cs           ← 處理呼叫 Python、格式化資料
│
├── PythonCrawler/                  ← 專門放 Python 爬蟲程式碼
│   ├── crawler.py                  ← 主爬蟲腳本
│   └── helpers/
│       └── parser.py               ← 可拆成工具函式
│
├── Data/                           ← 儲存匯出結果（可選）
│   └── output.json
│
├── wwwroot/
├── Program.cs
├── Startup.cs
└── MyProject.csproj

## py 指令
若使用 pyenv 管理版本，最好每個專案搭配虛擬環境（如 venv 或 pyenv-virtualenv）來避免版本衝突。

### pyenv: Python 版本管理工具（用來在一台機器上安裝並切換多個 Python 版本。）
#### 如果要更新 pyenv 可使用下面指令
使用 homebrew 指令安裝
brew update # 保持最新版本

##### 如果沒有安裝 pyenv
brew install pyenv
設定 shell 環境

nano ~/.zshrc
```
export PYENV_ROOT="$(brew --prefix)/opt/pyenv"
export PATH="$PYENV_ROOT/bin:$PATH"
eval "$(pyenv init --path)"
eval "$(pyenv init -)"
```
存檔： Control 鍵和字母 O
確認： Enter 確認
退出： Ctrl + X 退出 nano 編輯器
執行： source ~/.zshrc
確認： pyenv --version


##### 如果有安裝 pyenv
brew upgrade pyenv

指令	說明
```
pyenv install --list | grep -E '^\s*3\.[0-9]+\.[0-9]+$' | tail -n 10    列出目前 python 版本前10
pyenv versions	列出所有已安裝的版本
pyenv install 3.11.6	安裝指定版本的 Python
pyenv global 3.11.6	設定全域使用的 Python 版本（切換）
pyenv local 3.10.0	為目前資料夾設定特定版本（會產生 .python-version）——帶表不是全域 當下資料夾
pyenv uninstall 3.10.0	移除已安裝的版本
pyenv doctor	檢查安裝環境是否正常（需安裝插件）
```
### pip: Python 套件管理工具（用來安裝、更新、移除 Python 套件）
```
pip freeze	                     #顯示當前環境已安裝的所有套件（含版本）
pip list	                     #列出所有已安裝的套件
pip install requests==2.31.0	 #安裝指定版本
pip install requests	         #安裝 requests 套件
pip install -r requirements.txt	 #安裝一個需求檔中列出的所有套件
pip uninstall requests	         #移除套件
```
### 搭配範例：
#### 安裝指定 Python 版本
```
pyenv install 3.11.6
pyenv local 3.11.6
```
#### 建立虛擬環境
```
python -m venv venv
source venv/bin/activate
```
#### 安裝套件
`pip install requests flask`

### PythonCrawler 建立虛擬環境
#### 建立虛擬環境（已建立過可跳過）
`python -m venv venv`
#### 啟動虛擬環境
MAC: `source venv/bin/activate`
#### 安裝套件（已安裝可跳過）
```
pip install playwright    
playwright install        #安裝 Playwright 瀏覽器核心（必須執行，Playwright 才能正常使用）
```

#### 腳本執行範例
python test_playwright.py

[playwright文件](https://playwright.dev/python/docs/intro)

## line Ｍessage
至 line notify 申請
https://developers.line.biz/zh-hant/services/messaging-api/