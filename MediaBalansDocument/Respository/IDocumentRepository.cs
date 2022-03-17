using MediaBalansDocument.Models;
using MediaBalansDocument.Wrappers;

namespace MediaBalansDocument.Respository;

public interface IDocumentRepository
{
    Task<CustomResponse<Document>> GetDocumentUniqueByAsync(string unique);
    Task<CustomResponse<string>> AddDocumentAsync(FileInput document);
    Task<CustomResponse<string>> UpdateDocumentAsync(string unique, FileInput document);
    Task<CustomResponse<bool>> RemoveDocumentAsync(string unique);

}
