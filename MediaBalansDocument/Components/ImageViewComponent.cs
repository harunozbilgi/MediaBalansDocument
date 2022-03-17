using MediaBalansDocument.Models;
using MediaBalansDocument.Respository;
using MediaBalansDocument.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MediaBalansDocument.Components;

public class ImageViewComponent : ViewComponent
{
    private readonly IDocumentRepository _documentRepository;
    private readonly DocumentSetting _documentSetting;
    public ImageViewComponent(IDocumentRepository documentRepository, IOptions<DocumentSetting> documentSetting)
    {
        _documentRepository = documentRepository;
        _documentSetting = documentSetting.Value;
    }
    public async Task<IViewComponentResult> InvokeAsync(string FileCode)
    {
        var result = await _documentRepository.GetDocumentUniqueByAsync(FileCode);
        if (result.IsSuccessful)
        {
            var file = result.Data;
            return View(new ImageViewModel()
            {
                Image = string.Concat(_documentSetting.StorageUrl, file.DocumentFolderName, file.DocumentName)
            });
        }
        return View(new ImageViewModel()
        {
            Image = ""
        });
    }
}
