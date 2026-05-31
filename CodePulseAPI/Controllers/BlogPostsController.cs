using CodePulseAPI.Models.DTO;
using CodePulseAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostService _blogPostService;

    public BlogPostsController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
    {
        var response = await _blogPostService.CreateAsync(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var response = await _blogPostService.GetAllAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
    {
        var response = await _blogPostService.GetByIdAsync(id);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto request)
    {
        var response = await _blogPostService.UpdateAsync(id, request);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
    {
        var response = await _blogPostService.DeleteAsync(id);
        return response is null ? NotFound() : Ok(response);
    }
}
