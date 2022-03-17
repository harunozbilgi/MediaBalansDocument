namespace MediaBalansDocument.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public string DocumentUnique { get; set; }=string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentSize { get; set; } = string.Empty;
        public string DocumentFolderName { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
        public string Video_Url { get; set; } = string.Empty;
    }
}
