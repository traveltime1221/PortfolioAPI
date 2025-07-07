using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Models;

namespace PortfolioAPI.Data;
public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    // 連接資料庫 Table 
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Holding> Holdings { get; set; }
}

