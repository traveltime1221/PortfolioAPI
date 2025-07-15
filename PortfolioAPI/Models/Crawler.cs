//namespace PortfolioAPI.Models;
//using Newtonsoft.Json.Linq;

public class EarthquakeTWInfo
{
    public string id { get; set; }
    public string 地震時間 { get; set; }
    public string 震央位置 { get; set; }
    public string 地震深度 { get; set; }
    public string 規模 { get; set; }
    public string 相對位置 { get; set; }
    public string 圖片 { get; set; }
}

public class EarthquakeTWInfoRes
{
    public string date { get; set; }
    public List<EarthquakeTWInfo> data { get; set; }
}

public class EarthquakeWorldInfo
{
    public string 地震時間 { get; set; }
    public string 經度 { get; set; }
    public string 緯度 { get; set; }
    public string 深度_公里 { get; set; }
    public string 規模 { get; set; }
    public string 地震位置 { get; set; }
}

public class TyphoonItem
{
    public string 地區 { get; set; }
    public string[] 資訊 { get; set; }
}

public class TyphoonInfo
{
    public string 更新時間 { get; set; }
    public string 颱風名稱 { get; set; }
    public List<TyphoonItem> 詳細資訊 { get; set; }
}

public class TyphoonInfoRes
{
    public string date { get; set; }
    public TyphoonInfo data { get; set; }
}

public class EarthquakeWorldInfoRes
{
    public string date { get; set; }
    public List<EarthquakeWorldInfo> data { get; set; }
}



