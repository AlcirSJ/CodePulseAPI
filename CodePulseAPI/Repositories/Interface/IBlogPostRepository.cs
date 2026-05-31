using CodePulseAPI.Models.Domain;

namespace CodePulseAPI.Repositories.Interface;

public interface IBlogPostRepository
{
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<(IEnumerable<BlogPost> Items, int TotalItems)> GetAllAsync(int page, int pageSize);
    Task<BlogPost?> GetByIdAsync(Guid id);
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    Task<BlogPost?> DeleteAsync(Guid id);
}
