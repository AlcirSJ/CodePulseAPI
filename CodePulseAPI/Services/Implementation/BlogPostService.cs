using CodePulseAPI.Models.Common;
using CodePulseAPI.Models.Domain;
using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using CodePulseAPI.Services.Interface;

namespace CodePulseAPI.Services.Implementation;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BlogPostService(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
    {
        _blogPostRepository = blogPostRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<BlogPostDto> CreateAsync(CreateBlogPostRequestDto request)
    {
        var blogPost = new BlogPost
        {
            Author = request.Author,
            Title = request.Title,
            Content = request.Content,
            IsVisible = request.IsVisible,
            ShortDescription = request.ShortDescription,
            FeaturedImageUrl = request.FeaturedImageUrl,
            PublishedDate = request.PublishedDate,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        foreach (var categoryId in request.Categories)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is not null)
                blogPost.Categories.Add(category);
        }

        blogPost = await _blogPostRepository.CreateAsync(blogPost);
        return MapToDto(blogPost);
    }

    public async Task<PagedResult<BlogPostDto>> GetAllAsync(int page, int pageSize)
    {
        var (items, totalItems) = await _blogPostRepository.GetAllAsync(page, pageSize);
        return new PagedResult<BlogPostDto>
        {
            Items = items.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task<BlogPostDto?> GetByIdAsync(Guid id)
    {
        var blogPost = await _blogPostRepository.GetByIdAsync(id);
        return blogPost is null ? null : MapToDto(blogPost);
    }

    public async Task<BlogPostDto?> UpdateAsync(Guid id, UpdateBlogPostRequestDto request)
    {
        var blogPost = new BlogPost
        {
            Id = id,
            Author = request.Author,
            Title = request.Title,
            Content = request.Content,
            IsVisible = request.IsVisible,
            ShortDescription = request.ShortDescription,
            FeaturedImageUrl = request.FeaturedImageUrl,
            PublishedDate = request.PublishedDate,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        foreach (var categoryId in request.Categories)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is not null)
                blogPost.Categories.Add(category);
        }

        var updated = await _blogPostRepository.UpdateAsync(blogPost);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<BlogPostDto?> DeleteAsync(Guid id)
    {
        var deleted = await _blogPostRepository.DeleteAsync(id);
        return deleted is null ? null : MapToDto(deleted);
    }

    private static BlogPostDto MapToDto(BlogPost blogPost) => new()
    {
        Id = blogPost.Id,
        Author = blogPost.Author,
        Title = blogPost.Title,
        Content = blogPost.Content,
        IsVisible = blogPost.IsVisible,
        ShortDescription = blogPost.ShortDescription,
        FeaturedImageUrl = blogPost.FeaturedImageUrl,
        PublishedDate = blogPost.PublishedDate,
        UrlHandle = blogPost.UrlHandle,
        Categories = blogPost.Categories?.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            UrlHandle = x.UrlHandle
        }).ToList() ?? []
    };
}
