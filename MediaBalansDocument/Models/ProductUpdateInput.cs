using System.ComponentModel.DataAnnotations;

namespace MediaBalansDocument.Models;

public class ProductUpdateInput
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Urun Adi")]
    [StringLength(15, ErrorMessage = "Lutfen 15 karakterden yukari girmeyiniz")]
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public List<ProductFile> ProductFiles { get; set; }

    [Display(Name = "Gorsel")]
    public IFormFile File { get; set; }
    [Display(Name = "Detay Resimleri")]
    public List<IFormFile> Files { get; set; }
}


