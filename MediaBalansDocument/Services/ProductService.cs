using MediaBalansDocument.Data;
using MediaBalansDocument.Library;
using MediaBalansDocument.Models;
using MediaBalansDocument.Respository;
using MediaBalansDocument.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace MediaBalansDocument.Services;

public class ProductService : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CustomResponse<Product>> AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return CustomResponse<Product>.Success(product);
    }

    public async Task AddProductFileAsync(ProductFile product)
    {
        _context.ProductFiles.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<CustomResponse<Product>> GetProductByIdAsync(Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
        var response = await _context.Products.Where(x => x.Id == productId).Include(x => x.ProductFiles).AsNoTracking().FirstOrDefaultAsync();
        if (response == null)
        {
            return CustomResponse<Product>.Fail("Hatali istek");
        }
        return CustomResponse<Product>.Success(response);
    }

    public async Task<CustomResponse<List<Product>>> GetProductsAsync()
    {
        var result = await _context.Products.Include(x => x.ProductFiles).AsNoTracking().ToListAsync();
        return CustomResponse<List<Product>>.Success(result);
    }

    public async Task<CustomResponse<bool>> RemoveProductFileAsync(Guid productId)
    {
        var result = await _context.ProductFiles.Where(x => x.ProductId == productId).ToListAsync();
        _context.ProductFiles.RemoveRange(result);
        await _context.SaveChangesAsync();
        return CustomResponse<bool>.Success(true);
    }

    public async Task<CustomResponse<bool>> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return CustomResponse<bool>.Success(true);
    }
}
