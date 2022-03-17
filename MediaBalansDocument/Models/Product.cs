namespace MediaBalansDocument.Models;

public class Product
{
    public Product()
    {
        ProductFiles = new HashSet<ProductFile>();
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public virtual ICollection<ProductFile> ProductFiles { get; set; }

}
