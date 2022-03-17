namespace MediaBalansDocument.Models
{
    public class ProductFile
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string PictureUrl { get; set; }
        public virtual Product Product { get; set; }    
    }
}
