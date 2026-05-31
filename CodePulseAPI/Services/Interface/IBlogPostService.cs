using CodePulseAPI.Models.Common;
using CodePulseAPI.Models.DTO;

namespace CodePulseAPI.Services.Interface;

public interface IBlogPostService
{
    Task<BlogPostDto> CreateAsync(CreateBlogPostRequestDto request);
    Task<PagedResult<BlogPostDto>> GetAllAsync(int page, int pageSize);
    Task<BlogPostDto?> GetByIdAsync(Guid id);
    Task<BlogPostDto?> UpdateAsync(Guid id, UpdateBlogPostRequestDto request);
    Task<BlogPostDto?> DeleteAsync(Guid id);
}
