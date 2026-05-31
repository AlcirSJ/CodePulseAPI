using System.ComponentModel.DataAnnotations;

namespace CodePulseAPI.Models.DTO;

public class UpdateCategoryRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "UrlHandle must be lowercase letters, numbers and hyphens only.")]
    public string UrlHandle { get; set; }
}
