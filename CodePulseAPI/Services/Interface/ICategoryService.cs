using CodePulseAPI.Models.Common;
using CodePulseAPI.Models.DTO;

namespace CodePulseAPI.Services.Interface;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request);
    Task<PagedResult<CategoryDto>> GetAllAsync(int page, int pageSize);
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryRequestDto request);
    Task<CategoryDto?> DeleteAsync(Guid id);
}
