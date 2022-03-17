namespace MediaBalansDocument.Models;

public class FileInput
{
    public IFormFile File { get; set; }
    public string Path { get; set; } = string.Empty;
}
