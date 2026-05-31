using CodePulseAPI.Models.Common;
using CodePulseAPI.Models.Domain;
using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using CodePulseAPI.Services.Interface;

namespace CodePulseAPI.Services.Implementation;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request)
    {
        var category = new Category
        {
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        await _categoryRepository.CreateAsync(category);

        return MapToDto(category);
    }

    public async Task<PagedResult<CategoryDto>> GetAllAsync(int page, int pageSize)
    {
        var (items, totalItems) = await _categoryRepository.GetAllAsync(page, pageSize);
        return new PagedResult<CategoryDto>
        {
            Items = items.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category is null ? null : MapToDto(category);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryRequestDto request)
    {
        var category = new Category
        {
            Id = id,
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        var updated = await _categoryRepository.UpdateAsync(category);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<CategoryDto?> DeleteAsync(Guid id)
    {
        var deleted = await _categoryRepository.DeleteAsync(id);
        return deleted is null ? null : MapToDto(deleted);
    }

    private static CategoryDto MapToDto(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle
    };
}
