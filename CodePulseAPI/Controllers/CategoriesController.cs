using CodePulseAPI.Models.DTO;
using CodePulseAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
    {
        var response = await _categoryService.CreateAsync(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var response = await _categoryService.GetAllAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var response = await _categoryService.GetByIdAsync(id);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
    {
        var response = await _categoryService.UpdateAsync(id, request);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var response = await _categoryService.DeleteAsync(id);
        return response is null ? NotFound() : Ok(response);
    }
}
