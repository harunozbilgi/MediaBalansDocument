using Microsoft.AspNetCore.Http;
namespace MediaBalansDocument.Library;

public class FileUploader
{
    private static readonly string local_path = "wwwroot/";
    public static async Task<FileDto> UploadAsync(IFormFile file, string source_path, CancellationToken cancellationToken = default)
    {
        if (file is not null)
        {
            if (!IsFolderValid(file))
            {
                throw new Exception("Lütfen 3 mb yukarı resim yüklemeyiniz.");
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string guidId = GetGuId();
            string filename = string.Concat(guidId, Path.GetExtension(file.FileName));
            using var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create);
            await file.CopyToAsync(fileStream, cancellationToken);
            var response = new FileDto { Unique = guidId, DocumentName = filename, DocumentType = Path.GetExtension(file.FileName), DocumentSize = BytesToString(file.Length) };
            return response;
        }
        throw new Exception("Beklenmedik hata meydana geldi");
    }
    public static bool FolderRemove(string source_path)
    {
        if (!string.IsNullOrEmpty(source_path))
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }
        return false;
    }
    private static string GetGuId()
    {
        return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
    }
    private static bool IsFolderValid(IFormFile file)
    {
        return file.Length <= 3 * 1024 * 1024;
    }
    private static string BytesToString(long byteCount)
    {
        string[] suf = { "Byte", "KB", "MB", "GB", "TB", "PB", "EB" };
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
    }
}
