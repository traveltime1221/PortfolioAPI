namespace PortfolioAPI.Models;

public enum PortfolioActionType
{
    Create,
    Update,
    Delete,
}

public class PortfolioAction: PortfolioItem
{
    public string Action { get; set; }
}

public class PortfolioItem
{
    public int id { get; set; }
    public string title { get; set; }
    public string? description { get; set; }
    public string? image_url { get; set; }
    public string[] tags { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string? document_path { get; set; }
    public string? external_link { get; set; }
   
}