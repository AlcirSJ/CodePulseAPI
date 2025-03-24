using CodePulseAPI.Models.Domain;
using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;

namespace CodePulseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ICategoryRepository _categoryRepository;
    public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
    {
       _blogPostRepository = blogPostRepository;
       _categoryRepository = categoryRepository;
    }
    // POST: {apibaseurl}/api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
    {
        //Dto to domain model
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

        foreach(var categoryGuid in request.Categories)
        {
            var existingCategory = await _categoryRepository.GetById(categoryGuid);

            if (existingCategory is not null)
            {
                blogPost.Categories.Add(existingCategory);
            }
        }

        blogPost = await _blogPostRepository.CreateAsync(blogPost);

        //Domain model to Dto
        var response = new BlogPostDto
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
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);
    }

    // GET: {apibaseurl}/api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
       var blogPosts =  await _blogPostRepository.GetAllAsync();

        var response = new List<BlogPostDto>();
        foreach(var blogPost in blogPosts)
        {
            response.Add(new BlogPostDto
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
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            });
        }

        return Ok(response);
    }

    // GET: {apibaseurl}/api/blogposts
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetAllBlogPostById([FromRoute] Guid id)
    {
        var blogPost = await _blogPostRepository.GetByIdAsync(id);

        if(blogPost is null)
        {
            return NotFound();
        }

        var response = new BlogPostDto()       
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
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
        };
        

        return Ok(response);
    }

    //PUT: {apibaseurl}/api/blogposts/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
    {
        // Convert DTO to Domain
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

        foreach(var categoryGuid in request.Categories)
        {
            var existingCategory = await _categoryRepository.GetById(categoryGuid);

            if(existingCategory is not null)
            {
                blogPost.Categories.Add(existingCategory);
            }
        }

        var updatedBloggPost = await _blogPostRepository.UpdateAsync(blogPost);

        if(updatedBloggPost == null)
        {
            return NotFound();
        }

        var response = new BlogPostDto()
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
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);



    }
}
