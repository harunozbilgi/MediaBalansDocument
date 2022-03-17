using MediaBalansDocument.Models;
using MediaBalansDocument.Wrappers;

namespace MediaBalansDocument.Respository;

public interface IProductRepository
{
    Task<CustomResponse<List<Product>>> GetProductsAsync();
    Task<CustomResponse<Product>> GetProductByIdAsync(Guid productId);
    Task<CustomResponse<Product>> AddProductAsync(Product product);
    Task<CustomResponse<bool>> UpdateProductAsync(Product product);
    Task AddProductFileAsync(ProductFile product);
    Task<CustomResponse<bool>> RemoveProductFileAsync(Guid productId);

}
