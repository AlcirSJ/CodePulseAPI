using System.ComponentModel.DataAnnotations;

namespace CodePulseAPI.Models.DTO;

public class UpdateBlogPostRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    [MaxLength(500)]
    public string ShortDescription { get; set; }

    [Required]
    public string Content { get; set; }

    [Url]
    public string? FeaturedImageUrl { get; set; }

    [Required]
    [MaxLength(200)]
    [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "UrlHandle must be lowercase letters, numbers and hyphens only.")]
    public string UrlHandle { get; set; }

    [Required]
    public DateTime PublishedDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Author { get; set; }

    public bool IsVisible { get; set; }

    public List<Guid> Categories { get; set; } = [];
}
