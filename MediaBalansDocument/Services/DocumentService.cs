using MediaBalansDocument.Data;
using MediaBalansDocument.Library;
using MediaBalansDocument.Library.Helpers;
using MediaBalansDocument.Models;
using MediaBalansDocument.Respository;
using MediaBalansDocument.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace MediaBalansDocument.Services;

public class DocumentService : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CustomResponse<string>> AddDocumentAsync(FileInput document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }
        var response = await FileUploader.UploadAsync(document.File, document.Path);
        var newPath = string.Concat(document.Path, response.DocumentName);
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response));
        }
        _context.Documents.Add(new Document
        {
            DocumentUnique = response.Unique,
            DocumentFolderName = document.Path,
            DocumentName = response.DocumentName,
            DocumentSize = response.DocumentSize,
            DocumentType = response.DocumentType,
        });
        await _context.SaveChangesAsync();
        await ImageHelper.OptimizeAsync(newPath);
        return CustomResponse<string>.Success(response.Unique);
    }

    public async Task<CustomResponse<Document>> GetDocumentUniqueByAsync(string unique)
    {
        if (unique is not null)
        {
            var result = await _context.Documents.Where(x => x.DocumentUnique == unique).FirstOrDefaultAsync();
            if (result is null)
            {
                return CustomResponse<Document>.Fail("Hatali istek");
            }
            return CustomResponse<Document>.Success(result);
        }
        throw new Exception("Bos deger gonderdiniz");
    }

    public async Task<CustomResponse<bool>> RemoveDocumentAsync(string unique)
    {
        if (unique is not null)
        {
            var result = await _context.Documents.Where(x => x.DocumentUnique == unique).FirstOrDefaultAsync();
            if (result is null)
            {
                return CustomResponse<bool>.Fail("Hatali istek");
            }
            var path = $"{result.DocumentFolderName}{result.DocumentName}";
            var delete = FileUploader.FolderRemove(path);
            return CustomResponse<bool>.Success(true);
        }
        throw new Exception("Beklenmedik hata");
    }

    public async Task<CustomResponse<string>> UpdateDocumentAsync(string unique, FileInput document)
    {
        if (unique is not null)
        {
            var result = await _context.Documents.Where(x => x.DocumentUnique == unique).FirstOrDefaultAsync();
            if (result is null)
            {
                return CustomResponse<string>.Fail("Hatali istek");
            }
            var path = $"{result.DocumentFolderName}{result.DocumentName}";
            var delete = FileUploader.FolderRemove(path);
            if (delete)
            {
                var response = await FileUploader.UploadAsync(document.File, document.Path);
                if (response == null)
                {
                    throw new Exception(nameof(response));
                }
                _context.Documents.Add(new Document
                {
                    DocumentUnique = response.Unique,
                    DocumentFolderName = document.Path,
                    DocumentName = response.DocumentName,
                    DocumentSize = response.DocumentSize,
                    DocumentType = response.DocumentType,
                });
                await _context.SaveChangesAsync();
                return CustomResponse<string>.Success(response.Unique);
            }
            throw new Exception("Hata meydana geldi");
        }
        var result_add = await AddDocumentAsync(document);
        return CustomResponse<string>.Success(result_add.Data);
    }
}
