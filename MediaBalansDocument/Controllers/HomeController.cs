using MediaBalansDocument.Models;
using MediaBalansDocument.Respository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MediaBalansDocument.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDocumentRepository _documentRepository;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger,
        IDocumentRepository documentRepository,
        IProductRepository productRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<IActionResult> Index()
    {
        var urunlist = await _productRepository.GetProductsAsync();
        return View(urunlist.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    #region Product
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateInput product)
    {

        var new_image_url = "";
        if (product.File != null)
        {
            var response = await _documentRepository.AddDocumentAsync(new FileInput() { File = product.File, Path = "Product/" });
            new_image_url += response.Data;
        }
        var add_product = new Product()
        {
            Name = product.Name,
            PictureUrl = new_image_url
        };
        var result = await _productRepository.AddProductAsync(add_product);
        if (result.IsSuccessful)
        {
            if (product.Files != null)
            {
                List<string> list_new_file = new();
                if (product.Files != null)
                {
                    foreach (var file in product.Files)
                    {
                        var response = await _documentRepository.AddDocumentAsync(new FileInput() { File = file, Path = "Product/" });
                        list_new_file.Add(response.Data);
                    }
                }
                foreach (var item in list_new_file)
                {
                    await _productRepository.AddProductFileAsync(new ProductFile
                    {
                        PictureUrl = item,
                        ProductId = result.Data.Id
                    });
                }
            }

        }
        return RedirectToAction(nameof(Index));

    }


    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();
        var response = await _productRepository.GetProductByIdAsync(id);
        var product = new ProductUpdateInput()
        {
            Id = response.Data.Id,
            Name = response.Data.Name,
            PictureUrl = response.Data.PictureUrl,
            ProductFiles = response.Data.ProductFiles.ToList(),
        };
        return View(product);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUpdateInput product)
    {

        if (product.File != null)
        {
            var response = await _documentRepository.UpdateDocumentAsync(product.PictureUrl, new FileInput() { File = product.File, Path = "Product/" });
            product.PictureUrl += response.Data;

        }
        var update_product = new Product()
        {

            Name = product.Name,
            PictureUrl = product.PictureUrl,
            Id = product.Id
        };
        var result = await _productRepository.UpdateProductAsync(update_product);
        if (result.IsSuccessful)
        {
            if (product.Files != null)
            {
                var remove_path_file = await _productRepository.GetProductByIdAsync(product.Id);
                if (remove_path_file.IsSuccessful)
                {
                    foreach (var item in remove_path_file.Data.ProductFiles)
                    {
                        await _documentRepository.RemoveDocumentAsync(item.PictureUrl);
                    }

                    var remove_file = await _productRepository.RemoveProductFileAsync(product.Id);
                    if (remove_file.IsSuccessful)
                    {
                        List<string> list_new_file = new();
                        if (product.Files != null)
                        {
                            foreach (var file in product.Files)
                            {
                                var response = await _documentRepository.AddDocumentAsync(new FileInput() { File = file, Path = "Product/" });
                                list_new_file.Add(response.Data);
                            }
                        }
                        foreach (var item in list_new_file)
                        {
                            await _productRepository.AddProductFileAsync(new ProductFile
                            {
                                PictureUrl = item,
                                ProductId = product.Id
                            });
                        }
                    }
                }
            }
        }
        return RedirectToAction(nameof(Index));
    }

    #endregion


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
