using MediaBalansDocument.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaBalansDocument.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Document> Documents { get; set; }  
    public DbSet<Product> Products { get; set; }    
    public DbSet<ProductFile> ProductFiles { get; set; }    
}
