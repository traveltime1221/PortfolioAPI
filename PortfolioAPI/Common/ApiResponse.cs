namespace PortfolioAPI.Common;

public class ApiResponse
{
    public string Status { get; set; } // 狀態，例如 "success" 或 "error"
    public string Message { get; set; } // 回應描述
    public object Content { get; set; } // 回應資料
    public object ErrorDetail { get; set; } // 錯誤細節
    
    // 刪除狀態可能會有多個, 是否改成 ENUM： 1 | success | '1'
    // 狀態碼
    
    // 成功回應
    public static ApiResponse Success(object content = null, string message = "操作成功")
    {
        return new ApiResponse
        {
            Status = "success",
            Message = message,
            Content = content,
            ErrorDetail = null
        };
    }

    // 失敗回應
    public static ApiResponse Error(object errorDetail = null, string message = "操作失敗")
    {
        return new ApiResponse
        {
            Status = "error",
            Message = message,
            Content = null,
            ErrorDetail = errorDetail
        };
    }
}